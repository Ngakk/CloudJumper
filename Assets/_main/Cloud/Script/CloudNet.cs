using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CloudNet : NetworkBehaviour
{
    public MovementNet playerToFollow;

    public GameObject mainModel, transparentModel;

    public bool instantiator;

    private void Start()
    {
        if (instantiator)
        {
            mainModel.SetActive(true);
            transparentModel.SetActive(false);

            gameObject.layer = 9;
            ChangeChildLayers(transform, 9);
        }

        playerToFollow = StaticManager.localPlayer;
    }

    public override void OnStartClient()
    {
        playerToFollow = StaticManager.localPlayer;
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
                SelfDestroy();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.SendMessageUpwards("Jump", SendMessageOptions.DontRequireReceiver);
            SelfDestroy();
        }
    }

    void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
