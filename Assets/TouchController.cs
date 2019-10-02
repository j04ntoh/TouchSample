using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TouchScript;
using TouchScript.Pointers;

public class ObjectMarker
{
    public int PointerID = -1;
    public GameObject gameObj;

    public void Init(Transform layer, int id)
    {
        byte customColor = (byte)Random.Range(0, 255);
        gameObj.GetComponent<Image>().color = new Color32(customColor, customColor, customColor, 255);
        gameObj.SetActive(false);
        gameObj.transform.SetParent(layer, false);
    }
}

public class TouchController : MonoBehaviour
{
    public GameObject prefab;
    public Transform canvas;
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
        canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
        objectMarker = new ObjectMarker[2];

        for (int i = 0; i < objectMarker.Length; i++)
        {
            objectMarker[i] = new ObjectMarker();
            objectMarker[i].gameObj = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
            objectMarker[i].Init(canvas, i);
        }
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
                //    Debug.Log("ADDED: "+position + op.ObjectId.ToString());
                    int newid = InitPoint();
                    if(newid != -1)
                        objectMarker[newid].PointerID = op.ObjectId;
                    getMarker(op.ObjectId);
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
                    moveMarker(op.ObjectId, position);
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
                //    Debug.Log("REMOVED: "+position + op.ObjectId.ToString());
                    hideMarker(op.ObjectId);
                }
            }
        }
    }
    public int InitPoint()
    {
        for(int i=0; i<objectMarker.Length; i++)
        {
            if (!objectMarker[i].gameObj.activeInHierarchy)
            {
                return i;
            }
        }
        return -1;
    }
    public void RemovePoint(int id)
    {
        for (int i = 0; i < objectMarker.Length; i++)
        {
            if (objectMarker[i].PointerID == id)
                objectMarker[i].PointerID = -i;
        }
    }
    public void getMarker(int id)
    {
        foreach (ObjectMarker o in objectMarker)
        {
            if (o.PointerID == id)
            {
                o.gameObj.transform.SetAsLastSibling();
                o.gameObj.SetActive(true);
            }
        }
    }
    public void moveMarker(int id, Vector2 pos)
    {
        foreach (ObjectMarker o in objectMarker)
        {
            if (o.PointerID == id)
            {
                o.gameObj.transform.position = new Vector3(pos.x, pos.y, 1);
            }
        }

    }
    public void hideMarker(int id)
    {
        foreach (ObjectMarker o in objectMarker)
        {
            if (o.PointerID == id)
            {
                o.gameObj.SetActive(false);
            }
        }
    }
}
