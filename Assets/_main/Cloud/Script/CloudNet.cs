using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CloudNet : NetworkBehaviour
{
    public MovementNet playerToFollow;

    public GameObject mainModel, transparentModel;

    public int instantiator;

    private void Start()
    {
        //playerToFollow = StaticManager.localPlayer;
    }

    public override void OnStartClient()
    {
        //playerToFollow = StaticManager.localPlayer;
    }

    public void SetIsMain(bool _b)
    {
        if (_b)
        {
            mainModel.SetActive(true);
            transparentModel.SetActive(false);

            gameObject.layer = 9;
            ChangeChildLayers(transform, 9);
        }
        else
        {
            mainModel.SetActive(false);
            transparentModel.SetActive(true);

            gameObject.layer = 10;
            ChangeChildLayers(transform, 10);
        }
    }

    private void ChangeChildLayers(Transform _parent, int _layer)
    {
        for (int i = 0; i < _parent.childCount; i++)
        {
            Transform child = _parent.GetChild(i);
            child.gameObject.layer = _layer;
            ChangeChildLayers(child, _layer);
        }
    }

    private void Update() {
        if (playerToFollow != null)
        {
            if (playerToFollow.transform.position.y - 8 > transform.position.y)
            {
                Cmd_SelfDestroy();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.SendMessageUpwards("Jump", SendMessageOptions.DontRequireReceiver);
            mainModel.SetActive(false);
            transparentModel.SetActive(false);
            if (hasAuthority)
            {
                Cmd_SelfDestroy();
            }
        }
    }

    


    [Command]
    void Cmd_SelfDestroy()
    {
        Destroy(gameObject);
    }
}
