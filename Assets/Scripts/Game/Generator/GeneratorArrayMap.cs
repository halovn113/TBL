using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

// class vẫn còn trong giai đoạn test và dần hoàn thiện -- WIP
public class GeneratorArrayMap : MonoBehaviour
{
    public int Doors;
    public int Width;
    public int Height;
    
    public List<GameObject> roomObject1x1;
    public List<GameObject> roomObject1x2;
    public List<GameObject> roomObject2x2;
    public GameObject parentTest;
    public RawData focusPoint;

    [Serializable]
    public struct RawData
    {
        public int x;
        public int y;
        [HideInInspector]
        public int data;
    }

    public enum Option
    {
        Force4Direction = 1,
        CenterDirection = 2,
        Random4Direction = 4,
        Queue4Direction = 8,
        HasFocusPoint = 16,

    }

    public int count = 4; // test

    [EnumFlag]
    public Option option;

    private RawData[,] arrayData;

#if UNITY_EDITOR
    //[CustomEditor(typeof(GeneratorArrayMap))]
    //public class CreateMapTest : Editor
    //{
    //    public override void OnInspectorGUI()
    //    {
    //        var script = target as GeneratorArrayMap;
    //        if ((script.option & Option.HasFocusPoint) != 0)
    //        {
    //            script.focusPoint.x = EditorGUILayout.IntSlider("Focus point x: ", script.focusPoint.x, 1, 100);
    //            script.focusPoint.y = EditorGUILayout.IntSlider("Focus point y: ", script.focusPoint.y, 1, 100);

    //        }
    //    }
    //}
#endif

    [ContextMenu("Test show array")]
    public void PrintArray()
    {
        CreateEmptyArray();
        CreateArray(arrayData);
        for (int i = 0; i < Height; i++)
        {
            var sb = new System.Text.StringBuilder();   
            for (int j = 0; j < Width; j++)
            {
                sb.Append("   " + arrayData[i, j].data.ToString());
            }
            //Debug.Log(sb.ToString());
        }
        
        Vector2 startVec = new Vector2();
        float w = roomObject1x1[0].GetComponent<SpriteRenderer>().bounds.size.x;
        float h = roomObject1x1[0].GetComponent<SpriteRenderer>().bounds.size.y;
        startVec.x = 0 - ((Width / 2) * w);
        startVec.y = 0 + ((Height / 2) * h);

        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                if (arrayData[i, j].data == 1) 
                {
                    GameObject go = Instantiate(roomObject1x1[0]);
                    Vector2 pos = new Vector3();
                    pos.x = startVec.x + (w * j);
                    pos.y = startVec.y - (h * i);
                    go.transform.position = pos;
                    go.transform.parent = parentTest.transform;
                }
      
            }
        }
        //Debug.Log("prop " + this.GetType().GetProperties());
        var e = this.GetType().GetFields();
        var e2 = GetComponent<Collider2D>().GetType().GetFields();
        object oj = this;
        Debug.Log("fields ");
        foreach (var g in e)
        {
            Debug.Log("g " + g.Name + " // " + g.GetValue(this));
        }

        foreach (var gg in e2)
        {
            Debug.Log("gg " + gg.Name + " // " + gg.GetValue(GetComponent<Collider2D>()));
        }


        Debug.Log("Prop");
        var v = GetType().GetProperties();
        var v2 = GetComponent<Collider2D>().GetType().GetProperties();
        Debug.Log("v2 lenght " + v2.Length);

        foreach (var vg in v2)
        {
            Debug.Log("vg " + vg.Name + " // " + vg.GetValue(GetComponent<Collider2D>(), null));
        }
        foreach (var vv in v)
        {
            Debug.Log("vv " + vv.Name + " // " + vv.GetValue(this, null));
        }



        Debug.Log("test.................");
        //var gg = Utility.
    }

    public void GenerateMap()
    {

    }

    [ContextMenu("Test_DestroyChild")]
    public void DestroyChild()
    {
        if (parentTest.transform.childCount > 0)
        {
            foreach (Transform child in parentTest.transform)
            {
                DestroyImmediate(child.gameObject);
            }
        }
    }

    void CreateEmptyArray()
    {
        arrayData = new RawData[Height, Width];
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                arrayData[i, j].data = 0;
                arrayData[i, j].x = i;
                arrayData[i, j].y = j;
            }
        }
    }

    public struct Point
    {
        public int x;
        public int y;
    }

    void CreateArray(RawData[,] arrayData)
    {
        RawData end = new RawData();
        RawData focus = new RawData();
        Point eP = new Point();
        Point fP = new Point();

        #region
        // Action work
        Action LeftToPoint = () => { arrayData = PointToPoint(arrayData[UnityEngine.Random.Range(0, Height), end.x - 1 <= 0? 0 : UnityEngine.Random.Range(0, end.x - 1)], end, arrayData); }; // left
        Action RightToPoint = () => { arrayData = PointToPoint(arrayData[UnityEngine.Random.Range(0, Height), end.x + 1 >= Width - 1? Width - 1 : UnityEngine.Random.Range(end.x + 1, Width)], end, arrayData); }; // right
        Action UpToPoint = () => { arrayData = PointToPoint(arrayData[end.y - 1 <= 0? 0 : UnityEngine.Random.Range(0, end.y - 1), UnityEngine.Random.Range(0, Width)], end, arrayData); }; // up
        Action DownToPoint = () => { arrayData = PointToPoint(arrayData[end.y + 1 >= Height - 1? Height - 1 : UnityEngine.Random.Range(end.y + 1, Height), UnityEngine.Random.Range(0, Width)], end, arrayData); }; // down
        
        #endregion

        // force center
        // bắt buộc điểm đích là trung tâm
        if ((option & Option.CenterDirection) != 0)
        {
            eP.x = Height / 2;
            eP.y = Width / 2;
        }
        else
        {
            eP.x = UnityEngine.Random.Range(0, Height);
            eP.y = UnityEngine.Random.Range(0, Width);
        }

        if ((option & Option.HasFocusPoint) != 0)
        {
            fP.x = UnityEngine.Random.Range(0, Height);
            fP.y = UnityEngine.Random.Range(0, Width);
        }

        arrayData[eP.x, eP.y].data = 1;
        end = arrayData[eP.x, eP.y];

        // force 4 basic directions
        // ở đây sẽ ép buộc array luôn có 4 hướng, không thể thiếu một được
        if ((option & Option.Force4Direction) != 0)
        {
            LeftToPoint();
            RightToPoint();
            UpToPoint();
            DownToPoint();
        }

        // queue 4 basic direction
        // phần này thì ngược lại, dùng cho trường hợp không bắt buộc phải có 4 hướng cơ bản
        // ví dụ sử dụng script này trong trường hợp chỉ sử dụng ít hơn 4 door và ngẫu nhiên ít hơn 4 cửa không đoán trước được 

        if ((option & Option.Queue4Direction) != 0)
        {
            Queue<Action> queue = new Queue<Action>();
            List<Action> action = new List<Action>();
            action.Add(LeftToPoint);
            action.Add(RightToPoint);
            action.Add(UpToPoint);
            action.Add(DownToPoint);

            while (action.Count != 0)
            {
                int i = UnityEngine.Random.Range(0, action.Count);
                queue.Enqueue(action[i]);
                action.RemoveAt(i);
            }

            foreach (Action ac in queue)
            {
                ac.Invoke();
            }
        }

        // random 4 basic direction
        // sử dụng để tạo các cửa ngẫu nhiên nhưng vẫn thuộc 4 hướng
        // có thể có các cửa trùng hướng
        if ((option & Option.Random4Direction) != 0)
        {
            Queue<Action> queue = new Queue<Action>();
            List<Action> action = new List<Action>();

            action.Add(LeftToPoint);
            action.Add(RightToPoint);
            action.Add(UpToPoint);
            action.Add(DownToPoint);

            for (int j = 0; j < count; j++)
            {
                int i = UnityEngine.Random.Range(0, action.Count);
                queue.Enqueue(action[i]);
            }

            foreach (Action ac in queue)
            {
                ac.Invoke();
            }
        }
    }

    RawData[,] PointToPoint(RawData start, RawData end, RawData[,] data, [System.Runtime.InteropServices.Optional] Nullable<bool> isHorizontal)
    {
        Debug.Log("start  x " + start.x + " , y " + start.y);
        if (data == null || data.Length == 0)
        {
            Debug.LogWarning("Warning, data is null or doesn't have any thing in there");
            return null;
        }
        RawData[,] d = data;
        //bool horizontal = isHorizontal.HasValue? (bool)isHorizontal : (UnityEngine.Random.Range(0, 10) % 2 == 0 ? true : false);
        bool horizontal = UnityEngine.Random.Range(0, 11) % 2 == 0 ? true : false;
        int i = start.x;
        int j = start.y;
        int goIn;
        Action hoAc = () =>
        {
            goIn = start.y < end.y ? 1 : -1;
            while (j != end.y)
            {
                d[i, j].data = 1;
                j += goIn;
            }
        };

        Action verAc = () =>
        {
            goIn = start.x < end.x ? 1 : -1;
            while (i != end.x)
            {
                d[i, j].data = 1;
                i += goIn;
            }
        };
        if (horizontal)
        {
            hoAc();
            verAc();
            //Debug.Log("horizontal");
        }
        else
        {
            verAc();
            hoAc();
            //Debug.Log("vertical");
        }
        return d;
    }


}
