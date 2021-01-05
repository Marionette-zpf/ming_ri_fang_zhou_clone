using LitJson;
using Module.Story;
using Module.Story.Cache;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class StoryEditor : EditorWindow
{
    [MenuItem("Tools/Story Editor")]
    static void Init()
    {
        StoryEditor window = (StoryEditor)EditorWindow.GetWindow(typeof(StoryEditor));
        window.Show();
    }

    private TextAsset m_storyData;

    private DialogFragment m_dialogFragment;

    private void OnGUI()
    {
        m_storyData = (TextAsset)EditorGUILayout.ObjectField("JsonData", m_storyData, typeof(TextAsset), false);

        if (m_storyData != null && GUILayout.Button("Load"))
        {
            LoadJsonData();
        }

        if (m_dialogFragment != null && GUILayout.Button("Save"))
        {
            SaveJsonData();
        }

        if (m_dialogFragment != null)
        {
            DrawJsonData();
        }
    }

    private void OnDestroy()
    {
        m_dialogFragment = null;
        m_storyData = null;
    }

    private void LoadJsonData()
    {
        m_dialogFragment = JsonMapper.ToObject<DialogFragment>(m_storyData.text);
    }

    private void SaveJsonData()
    {
        var savePath = AssetDatabase.GetAssetPath(m_storyData).Replace("Assets", string.Empty);

        var jsonData = JsonMapper.ToJson(m_dialogFragment);
        jsonData = System.Text.RegularExpressions.Regex.Unescape(jsonData);
        File.WriteAllText(Application.dataPath + savePath, jsonData);

        AssetDatabase.Refresh();
    }

    Vector2 scrollPos;

    private void DrawJsonData()
    {
        HorizontalCall(() =>
        {
            m_dialogFragment.Id = (uint)EditorGUILayout.IntField("FragmentId", (int)m_dialogFragment.Id);
        });

        using (var h = new EditorGUILayout.VerticalScope())
        {
            using (var scrollView = new EditorGUILayout.ScrollViewScope(scrollPos, GUILayout.Width(700), GUILayout.Height(400)))
            {
                scrollPos = scrollView.scrollPosition;

                if (m_dialogFragment.DialogConfigs != null)
                {
                    for (int i = 0; i < m_dialogFragment.DialogConfigs.Count; i++)
                    {
                        var dialogCfg = m_dialogFragment.DialogConfigs[i];
                        HorizontalCall(() =>
                        {
                            dialogCfg.Id = (uint)EditorGUILayout.IntField("    DialogId", (int)dialogCfg.Id);
                            dialogCfg.CharacterId = (uint)EditorGUILayout.IntField("    CharacterId", (int)dialogCfg.CharacterId);
                            dialogCfg.OptionId = (uint)EditorGUILayout.IntField("    OptionId", (int)dialogCfg.OptionId);
                        });

                        dialogCfg.Dialog = EditorGUILayout.TextField("    Dialog", dialogCfg.Dialog);
                        dialogCfg.DialogBg = EditorGUILayout.TextField("    DialogBg", dialogCfg.DialogBg);

                        if (dialogCfg.CharacterPainting != null || dialogCfg.GreyPainting != null)
                        {
                            int charPaintCount = dialogCfg.CharacterPainting.Count;

                            for (int j = 0; j < charPaintCount; j++)
                            {
                                var paint = dialogCfg.CharacterPainting[j];
                                var greyPaint = dialogCfg.GreyPainting[j];
                                HorizontalCall(() =>
                                {
                                    dialogCfg.CharacterPainting[j] = EditorGUILayout.TextField("    CharPaintUrl", paint);
                                    dialogCfg.GreyPainting[j] = EditorGUILayout.Toggle("    GreyPainting", greyPaint);
                                });
                            }
                        }

                        if(dialogCfg.DialogEvents != null)
                        {
                            for (int j = 0; j < dialogCfg.DialogEvents.Count; j++)
                            {
                                HorizontalCall(() =>
                                {
                                    dialogCfg.DialogEvents[j].Id = (uint)EditorGUILayout.IntField("    EventId", (int)dialogCfg.DialogEvents[j].Id); ;
                                    dialogCfg.DialogEvents[j].Parma = EditorGUILayout.TextField("    Param", dialogCfg.DialogEvents[j].Parma);
                                });
                            }
                        }

                        HorizontalCall(() =>
                        {
                            if (GUILayout.Button("Add Painting"))
                            {
                                if (dialogCfg.CharacterPainting == null || dialogCfg.GreyPainting == null)
                                {
                                    dialogCfg.CharacterPainting = new List<string>();
                                    dialogCfg.GreyPainting = new List<bool>();
                                }

                                dialogCfg.CharacterPainting.Add(string.Empty);
                                dialogCfg.GreyPainting.Add(false);
                            }

                            if (dialogCfg.CharacterPainting != null && dialogCfg.GreyPainting != null && GUILayout.Button("Del Painting"))
                            {
                                dialogCfg.CharacterPainting.RemoveAt(dialogCfg.CharacterPainting.Count - 1);
                                dialogCfg.GreyPainting.RemoveAt(dialogCfg.GreyPainting.Count - 1);

                                if(dialogCfg.CharacterPainting.Count == 0 || dialogCfg.GreyPainting.Count == 0)
                                {
                                    dialogCfg.CharacterPainting = null;
                                    dialogCfg.GreyPainting = null;
                                }
                            }
                        });



                        if (GUILayout.Button("Add Event"))
                        {
                            if(dialogCfg.DialogEvents == null)
                            {
                                dialogCfg.DialogEvents = new List<DialogEvent>();
                            }

                            dialogCfg.DialogEvents.Add(new DialogEvent());
                        }
                    }
                }

                if (GUILayout.Button("Add Dialog Fragment"))
                {
                    if (m_dialogFragment.DialogConfigs == null)
                    {
                        m_dialogFragment.DialogConfigs = new List<DialogConfig>();
                    }

                    m_dialogFragment.DialogConfigs.Add(new DialogConfig());
                }
            }
        }



    }
    private void HorizontalCall(Action call)
    {
        GUILayout.BeginHorizontal();
        call.Invoke();
        GUILayout.EndHorizontal();
    }
}
