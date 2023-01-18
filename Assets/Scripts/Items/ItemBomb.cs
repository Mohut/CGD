using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Items
{
    [CreateAssetMenu(fileName = "Bomb", menuName = "ScriptableObjects/Items/Bomb")]
    public class ItemBomb : Item
    {
        [SerializeField] private GameObject _bombPrefab;
        [SerializeField] private float _bombRadius;
        [SerializeField] private float timeToDetonate;


        public override void TriggerEffect(GameObject player)
        {
            // expensive invocation should be ok since items are not used often
            Color playerColor = player.GetComponent<PlayerDetails>().Color;
            // bomb should have the same color as the player who spawned it
            GameObject bombObject = GameObject.Instantiate(_bombPrefab, player.transform.position, Quaternion.identity);
            PlayerController controller = player.GetComponent<PlayerController>();
            bombObject.GetComponent<Bomb>().Trigger(playerColor, _bombRadius, timeToDetonate, controller.pathTilemap, controller.SetTileColour, player);
        }
    }
}