using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TouchScript;
using TouchScript.Pointers;

public class ObjectMarker
{
    public int MarkerID;
    public GameObject gameObj;
}

public class TouchController : MonoBehaviour
{
    public GameObject prefab;
    public ObjectMarker[] objectMarker;

    private static TouchController _instance;

    public static TouchController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<TouchController>();
            }
            return _instance;
        }
    }
    private void Start()
    {
        //for (int i = 0; i < objectMarker.Length; i++)
        //{
        //    marker.GetComponent<MarkerObjectClass>().MarkerGO.GetComponent<MarkerNode>().Init(
        //                                                                    Mathf.FloorToInt(i / 4),
        //                                                                    i % 4);
        //    objectMarker[i].gameObj = Instantiate(marker, new Vector3(0, 0, 0), Quaternion.identity);
        //    objectMarker[i].gameObj.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        //}
    }
    private void OnEnable()
    {
        if (TouchManager.Instance != null)
        {
            TouchManager.Instance.PointersAdded += pointersAddedHandler;
            TouchManager.Instance.PointersUpdated += pointerUpdatedHander;
            TouchManager.Instance.PointersRemoved += pointerRemoveHandler;
        }
    }

    private void OnDisable()
    {
        if (TouchManager.Instance != null)
        {
            TouchManager.Instance.PointersAdded -= pointersAddedHandler;
            TouchManager.Instance.PointersUpdated -= pointerUpdatedHander;
            TouchManager.Instance.PointersRemoved -= pointerRemoveHandler;
        }
    }    //---------------------------------------------

    void pointersAddedHandler(object sender, PointerEventArgs e)
    {
        lock (this)
        {
            foreach (var pointer in e.Pointers)
            {
                if (pointer.Type == Pointer.PointerType.Object)
                {
                    ObjectPointer op = (ObjectPointer)pointer;
                    Vector2 position = op.Position;
                    Debug.Log("ADDED: "+position + op.ObjectId.ToString());
                //    getMarker(op.ObjectId);
                }
            }
        }

    }

    void pointerUpdatedHander(object sender, PointerEventArgs e)
    {
        lock (this)
        {
            foreach (var pointer in e.Pointers)
            {
                if (pointer.Type == Pointer.PointerType.Object)
                {
                    ObjectPointer op = (ObjectPointer)pointer;
                    Vector2 position = op.Position;
                //    moveMarker(op.ObjectId, position);
                }
            }
        }
    }

    private void pointerRemoveHandler(object sender, PointerEventArgs e)
    {
        lock (this)
        {
            foreach (var pointer in e.Pointers)
            {
                if (pointer.Type == Pointer.PointerType.Object)
                {
                    ObjectPointer op = (ObjectPointer)pointer;
                    Vector2 position = op.Position;
                    Debug.Log("REMOVED: "+position + op.ObjectId.ToString());
                //    hideMarker(op.ObjectId);
                }
            }
        }
    }
    public void getMarker(int id)
    {
        foreach (ObjectMarker o in objectMarker)
        {
            if (o.MarkerID == id)
            {
                //    flagObject = true;
                o.gameObj.transform.SetAsLastSibling();
                o.gameObj.SetActive(true);
            }
        }
    }
    public void moveMarker(int id, Vector2 pos)
    {
        foreach (ObjectMarker o in objectMarker)
        {
            if (o.MarkerID == id)
            {
                o.gameObj.transform.position = new Vector3(pos.x, pos.y, 1);
            }
        }

    }
    public void hideMarker(int id)
    {
        foreach (ObjectMarker o in objectMarker)
        {
            if (o.MarkerID == id)
            {
                o.gameObj.SetActive(false);
            }
        }
    }
}
