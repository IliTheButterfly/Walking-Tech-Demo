﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
#endif

namespace Roads
{
#if UNITY_EDITOR
    [CustomEditor(typeof(PathRoadLayout))]
    [CanEditMultipleObjects]
    public class PathRoadLayoutDisplayer : Editor
    {
        Object selected;
        int stateHead = -1;
        int stateTail = -1;
        public override void OnInspectorGUI()
        {
            var t = (PathRoadLayout)target;
            base.OnInspectorGUI();
            //if (GUILayout.Button("Create road at start"))
            //{
            //    var pos = t[0];
            //    var obj = Instantiate(PathFactory.Instance.PathPrefab);
            //    var layout = obj.GetComponent<PathRoadLayout>();
            //    t.StartPaths.Add(layout);
            //    layout.ThisPath.bezierPath.AddSegmentToEnd(new Vector3(pos.x + 1, pos.y, pos.z));
            //    layout.ThisPath.bezierPath.AddSegmentToEnd(pos);
            //    layout.ThisPath.bezierPath.DeleteSegment(1);
            //    layout.ThisPath.bezierPath.DeleteSegment(0);
            //    layout.EndPaths.Add(t);
            //}
            //if (GUILayout.Button("Create road at end"))
            //{
            //    var pos = t[-1];
            //    var obj = Instantiate(PathFactory.Instance.PathPrefab);
            //    var layout = obj.GetComponent<PathRoadLayout>();
            //    t.EndPaths.Add(layout);
            //    layout.ThisPath.bezierPath.AddSegmentToEnd(pos);
            //    layout.ThisPath.bezierPath.AddSegmentToEnd(new Vector3(pos.x + 1, pos.y, pos.z));
            //    layout.ThisPath.bezierPath.DeleteSegment(1);
            //    layout.ThisPath.bezierPath.DeleteSegment(0);
            //    layout.StartPaths.Add(t);
            //}
            if (GUILayout.Button("Reset event"))
            {
                t.ResetHandle();
            }
            if (GUILayout.Button("Generate nodes"))
            {
                t.GenerateNodes();
            }
            if (GUILayout.Button("Update"))
            {
                t.EnableNotifications();
                t.OnUpdate();
            }
            GUILayout.Space(2);
            if (t.Head != null)
            {
                if (Event.current.commandName == "ObjectSelectorUpdated")
                    selected = EditorGUIUtility.GetObjectPickerObject();
                var head = t.Head;
                EditorGUILayout.PropertyField(new SerializedObject(t).FindProperty("_head"));
                if (GUILayout.Button("Connect path as incomming"))
                {
                    EditorGUIUtility.ShowObjectPicker<PathRoadLayout>(null, true, "", EditorGUIUtility.GetObjectPickerControlID());

                    stateHead = 0;
                }
                if (GUILayout.Button("Connect path as outgoing"))
                {
                    EditorGUIUtility.ShowObjectPicker<PathRoadLayout>(null, true, "", EditorGUIUtility.GetObjectPickerControlID());

                    stateHead = 1;
                }
                if (GUILayout.Button("Merge node to node"))
                {
                    EditorGUIUtility.ShowObjectPicker<PathNode>(null, true, "", EditorGUIUtility.GetObjectPickerControlID());

                    stateHead = 2;
                }

                if (Event.current.commandName == "ObjectSelectorClosed")
                {
                    switch (stateHead)
                    {
                        case 0:
                            {
                                var o = ((GameObject)selected).GetComponent<PathRoadLayout>();
                                head.AddAsIncomming(o);
                            }
                            break;
                        case 1:
                            {
                                var o = ((GameObject)selected).GetComponent<PathRoadLayout>();
                                head.AddAsOutgoing(o);
                            }
                            break;
                        case 2:
                            {
                                var o = ((GameObject)selected).GetComponent<PathNode>();
                                head.MergeTo(o);
                                Destroy(target);
                            }
                            break;
                        default:
                            break;
                    }
                    stateHead = -1;
                }

                if (GUILayout.Button("Create incomming road"))
                {
                    var pos = head.Position;
                    var obj = Instantiate(PathFactory.Instance.PathPrefab);
                    var layout = obj.GetComponent<PathRoadLayout>();
                    layout.Head = head;
                    layout.ThisPath.bezierPath.AddSegmentToEnd(new Vector3(pos.x + 1, pos.y, pos.z));
                    layout.ThisPath.bezierPath.AddSegmentToEnd(pos);
                    layout.ThisPath.bezierPath.DeleteSegment(1);
                    layout.ThisPath.bezierPath.DeleteSegment(0);
                }
                if (GUILayout.Button("Create outgoing road"))
                {
                    var pos = head.Position;
                    var obj = Instantiate(PathFactory.Instance.PathPrefab);
                    var layout = obj.GetComponent<PathRoadLayout>();
                    layout.Tail = head;
                    layout.ThisPath.bezierPath.AddSegmentToEnd(pos);
                    layout.ThisPath.bezierPath.AddSegmentToEnd(new Vector3(pos.x + 1, pos.y, pos.z));
                    layout.ThisPath.bezierPath.DeleteSegment(1);
                    layout.ThisPath.bezierPath.DeleteSegment(0);
                }
            }
            else
            {
                GUILayout.Label("No head");
            }
            GUILayout.Space(2);
            if (t.Tail != null)
            {
                if (Event.current.commandName == "ObjectSelectorUpdated")
                    selected = EditorGUIUtility.GetObjectPickerObject();
                var tail = t.Tail;
                EditorGUILayout.PropertyField(new SerializedObject(t).FindProperty("_tail"));
                if (GUILayout.Button("Connect path as incomming"))
                {
                    EditorGUIUtility.ShowObjectPicker<PathRoadLayout>(null, true, "", EditorGUIUtility.GetObjectPickerControlID());

                    stateTail = 0;
                }
                if (GUILayout.Button("Connect path as outgoing"))
                {
                    EditorGUIUtility.ShowObjectPicker<PathRoadLayout>(null, true, "", EditorGUIUtility.GetObjectPickerControlID());

                    stateTail = 1;
                }
                if (GUILayout.Button("Merge node to node"))
                {
                    EditorGUIUtility.ShowObjectPicker<PathNode>(null, true, "", EditorGUIUtility.GetObjectPickerControlID());

                    stateTail = 2;
                }

                if (Event.current.commandName == "ObjectSelectorClosed")
                {
                    switch (stateTail)
                    {
                        case 0:
                            {
                                var o = ((GameObject)selected).GetComponent<PathRoadLayout>();
                                tail.AddAsIncomming(o);
                            }
                            break;
                        case 1:
                            {
                                var o = ((GameObject)selected).GetComponent<PathRoadLayout>();
                                tail.AddAsOutgoing(o);
                            }
                            break;
                        case 2:
                            {
                                var o = ((GameObject)selected).GetComponent<PathNode>();
                                tail.MergeTo(o);
                                Destroy(target);
                            }
                            break;
                        default:
                            break;
                    }
                    stateTail = -1;
                }

                if (GUILayout.Button("Create incomming road"))
                {
                    var pos = tail.Position;
                    var obj = Instantiate(PathFactory.Instance.PathPrefab);
                    var layout = obj.GetComponent<PathRoadLayout>();
                    layout.Head = tail;
                    layout.ThisPath.bezierPath.AddSegmentToEnd(new Vector3(pos.x + 1, pos.y, pos.z));
                    layout.ThisPath.bezierPath.AddSegmentToEnd(pos);
                    layout.ThisPath.bezierPath.DeleteSegment(1);
                    layout.ThisPath.bezierPath.DeleteSegment(0);
                }
                if (GUILayout.Button("Create outgoing road"))
                {
                    var pos = tail.Position;
                    var obj = Instantiate(PathFactory.Instance.PathPrefab);
                    var layout = obj.GetComponent<PathRoadLayout>();
                    layout.Tail = tail;
                    layout.ThisPath.bezierPath.AddSegmentToEnd(pos);
                    layout.ThisPath.bezierPath.AddSegmentToEnd(new Vector3(pos.x + 1, pos.y, pos.z));
                    layout.ThisPath.bezierPath.DeleteSegment(1);
                    layout.ThisPath.bezierPath.DeleteSegment(0);
                }
            }
            else
            {
                GUILayout.Label("No tail");
            }
        }
    }
#endif
}
