using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

// Source code found here https://www.clonefactor.com/wordpress/public/1769/

namespace AnkleBreaker.Utils.Inspector
{
    public class AutoCompleteText
    {
        private const string AUTO_COMPLETE_FIELD = "AutoCompleteField";
        private static List<string> m_CacheCheckList = null;
        private static string m_AutoCompleteLastInput;
        private static string m_EditorFocusAutoComplete;

        /// <summary>A textField to popup a matching popup, based on developers input values.</summary>
        /// <param name="input">string input.</param>
        /// <param name="source">the data of all possible values (string).</param>
        /// <param name="maxShownCount">the amount to display result.</param>
        /// <param name="levenshteinDistance">
        /// value between 0f ~ 1f,
        /// - more then 0f will enable the fuzzy matching
        /// - 1f = anything thing is okay.
        /// - 0f = require full match to the reference
        /// - recommend 0.4f ~ 0.7f
        /// </param>
        /// <returns>output string.</returns>
        public static string Process(Rect position, string input, string[] source, int maxShownCount = 5,
            float levenshteinDistance = 0.5f, string[] toolTips = null, string textGUITag = null)
        {
            #if UNITY_EDITOR
            string rst = input;
            string tag = "";

            if (textGUITag != null)
            {
                tag = textGUITag;
            }
            else
            {
                tag = InitFieldTag();
                rst = EditorGUI.TextField(position, input);
            }

            int uiDepth = GUI.depth;

            if (input.Length > 0 && GUI.GetNameOfFocusedControl() == tag)
            {
                int lastWordIndex = -1;

                // Trim any trailing spaces and find the last space in the string
                input = input.TrimEnd();
                int lastSpaceIndex = input.LastIndexOf(' ');

                // Get the last word by substring
                string lastWord = lastSpaceIndex == -1 ? input : input.Substring(lastSpaceIndex + 1);

                // Get the index of the first character of the last word
                lastWordIndex = lastSpaceIndex == -1 ? 0 : lastSpaceIndex + 1;

                if (string.IsNullOrEmpty(lastWord))
                    return input;

                if (m_AutoCompleteLastInput != input || // input changed
                    m_EditorFocusAutoComplete != tag) // another field.
                {
                    // Update cache
                    m_EditorFocusAutoComplete = tag;
                    m_AutoCompleteLastInput = input;

                    List<string> uniqueSrc = new List<string>(new HashSet<string>(source)); // remove duplicate
                    int srcCnt = uniqueSrc.Count;
                    m_CacheCheckList =
                        new List<string>(System.Math.Min(maxShownCount, srcCnt)); // optimize memory alloc

                    // Start with - slow
                    for (int i = 0; i < srcCnt && m_CacheCheckList.Count < maxShownCount; i++)
                    {
                        string keyword = uniqueSrc[i];

                        if (keyword == lastWord)
                            break;

                        if (keyword.ToLower().StartsWith(lastWord.ToLower()))
                        {
                            m_CacheCheckList.Add(uniqueSrc[i]);
                            uniqueSrc.RemoveAt(i);
                            srcCnt--;
                            i--;
                        }
                    }

                    // Contains - very slow
                    if (m_CacheCheckList.Count == 0)
                    {
                        for (int i = 0; i < srcCnt && m_CacheCheckList.Count < maxShownCount; i++)
                        {
                            string keyword = uniqueSrc[i];

                            if (keyword == lastWord)
                                break;

                            if (keyword.ToLower().Contains(lastWord.ToLower()))
                            {
                                m_CacheCheckList.Add(uniqueSrc[i]);
                                uniqueSrc.RemoveAt(i);
                                srcCnt--;
                                i--;
                            }
                        }
                    }

                    // Levenshtein Distance - very very slow.
                    if (levenshteinDistance > 0f && // only developer request
                        lastWord.Length > 3 && // 3 characters on input, hidden value to avoid doing too early.
                        m_CacheCheckList.Count < maxShownCount) // have some empty space for matching.
                    {
                        levenshteinDistance = Mathf.Clamp01(levenshteinDistance);
                        string keywords = lastWord.ToLower();
                        for (int i = 0; i < srcCnt && m_CacheCheckList.Count < maxShownCount; i++)
                        {
                            string keyword = uniqueSrc[i];

                            if (keyword == lastWord)
                                break;

                            int distance = LevenshteinDistance(uniqueSrc[i], keywords, caseSensitive: false);
                            bool closeEnough = (int)(levenshteinDistance * uniqueSrc[i].Length) > distance;
                            if (closeEnough)
                            {
                                m_CacheCheckList.Add(uniqueSrc[i]);
                                uniqueSrc.RemoveAt(i);
                                srcCnt--;
                                i--;
                            }
                        }
                    }
                }

                // Draw recommend keyward(s)
                if (m_CacheCheckList.Count > 0)
                {
                    int cnt = m_CacheCheckList.Count;
                    float height = cnt * EditorGUIUtility.singleLineHeight;
                    Rect area = position;
                    area = new Rect(area.x, area.y - height, area.width, height);
                    GUI.depth -= 10;
                    // GUI.BeginGroup(area);
                    // area.position = Vector2.zero;
                    GUI.BeginClip(area);
                    Rect line = new Rect(0, 0, area.width, EditorGUIUtility.singleLineHeight);

                    for (int i = 0; i < cnt; i++)
                    {
                        string tooltip = null;
                        if (toolTips != null)
                        {
                            if (i < toolTips.Length)
                            {
                                tooltip = toolTips[i];
                            }
                        }

                        GUIContent content = new GUIContent(m_CacheCheckList[i], tooltip);

                        if (GUI.Button(line, content)) // Pass the GUIContent to the button
                        {
                            rst = rst.Substring(0, lastWordIndex);
                            rst = rst.Insert(lastWordIndex, m_CacheCheckList[i]);
                            GUI.changed = true;
                            GUI.FocusControl(""); // Force update
                        }

                        line.y += line.height;
                    }

                    GUI.EndClip();
                    //GUI.EndGroup();
                    GUI.depth += 10;
                }
            }

            return rst;
            
            #endif

            return string.Empty;
        }

        /// <summary>
        /// Call this before drawing the string field to identify it in the AutoCompleteText code
        /// </summary>
        /// <returns></returns>
        public static string InitFieldTag()
        {
            string tag;
            tag = AUTO_COMPLETE_FIELD + GUIUtility.GetControlID(FocusType.Passive);
            GUI.SetNextControlName(tag);
            return tag;
        }

        public static string Process(string input, string[] source, int maxShownCount = 5,
            float levenshteinDistance = 0.5f, string[] toolTips = null, string textGUITag = null)
        {
            #if UNITY_EDITOR
            if (input == null)
                return "";

            Rect rect = EditorGUILayout.GetControlRect();
            return Process(rect, input, source, maxShownCount, levenshteinDistance, toolTips, textGUITag);
            #endif

            return string.Empty;
        }

        /// <summary>Computes the Levenshtein Edit Distance between two enumerables.</summary>
        /// <typeparam name="T">The type of the items in the enumerables.</typeparam>
        /// <param name="lhs">The first enumerable.</param>
        /// <param name="rhs">The second enumerable.</param>
        /// <returns>The edit distance.</returns>
        /// <see cref="https://blogs.msdn.microsoft.com/toub/2006/05/05/generic-levenshtein-edit-distance-with-c/"/>
        public static int LevenshteinDistance<T>(IEnumerable<T> lhs, IEnumerable<T> rhs) where T : System.IEquatable<T>
        {
            // Validate parameters
            if (lhs == null) throw new System.ArgumentNullException("lhs");
            if (rhs == null) throw new System.ArgumentNullException("rhs");

            // Convert the parameters into IList instances
            // in order to obtain indexing capabilities
            IList<T> first = lhs as IList<T> ?? new List<T>(lhs);
            IList<T> second = rhs as IList<T> ?? new List<T>(rhs);

            // Get the length of both.  If either is 0, return
            // the length of the other, since that number of insertions
            // would be required.
            int n = first.Count, m = second.Count;
            if (n == 0) return m;
            if (m == 0) return n;

            // Rather than maintain an entire matrix (which would require O(n*m) space),
            // just store the current row and the next row, each of which has a length m+1,
            // so just O(m) space. Initialize the current row.
            int curRow = 0, nextRow = 1;

            int[][] rows = new int[][] { new int[m + 1], new int[m + 1] };
            for (int j = 0; j <= m; ++j)
                rows[curRow][j] = j;

            // For each virtual row (since we only have physical storage for two)
            for (int i = 1; i <= n; ++i)
            {
                // Fill in the values in the row
                rows[nextRow][0] = i;

                for (int j = 1; j <= m; ++j)
                {
                    int dist1 = rows[curRow][j] + 1;
                    int dist2 = rows[nextRow][j - 1] + 1;
                    int dist3 = rows[curRow][j - 1] +
                                (first[i - 1].Equals(second[j - 1]) ? 0 : 1);

                    rows[nextRow][j] = System.Math.Min(dist1, System.Math.Min(dist2, dist3));
                }

                // Swap the current and next rows
                if (curRow == 0)
                {
                    curRow = 1;
                    nextRow = 0;
                }
                else
                {
                    curRow = 0;
                    nextRow = 1;
                }
            }

            // Return the computed edit distance
            return rows[curRow][m];
        }

        /// <summary>Computes the Levenshtein Edit Distance between two enumerables.</summary>
        /// <param name="lhs">The first enumerable.</param>
        /// <param name="rhs">The second enumerable.</param>
        /// <returns>The edit distance.</returns>
        /// <see cref="https://en.wikipedia.org/wiki/Levenshtein_distance"/>
        public static int LevenshteinDistance(string lhs, string rhs, bool caseSensitive = true)
        {
            if (!caseSensitive)
            {
                lhs = lhs.ToLower();
                rhs = rhs.ToLower();
            }

            char[] first = lhs.ToCharArray();
            char[] second = rhs.ToCharArray();
            return LevenshteinDistance<char>(first, second);
        }
    }
}