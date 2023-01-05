using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Items
{
    public class Bomb : MonoBehaviour
    {
        public void Trigger(Color color, float radius, float timeToDetonate, Tilemap map, Action<Color, Vector3Int> callback, GameObject player)
        {
            Debug.Log("Trigger");
            StartCoroutine(tickToExplode(color, radius, timeToDetonate, map, callback, player));
        }

        IEnumerator tickToExplode(Color color, float radius, float _timeToDetonate, Tilemap map, Action<Color, Vector3Int> callback, GameObject player)
        {
            yield return new WaitForSeconds(_timeToDetonate);
            Explode(color, radius, map, callback, player);
        }

        private void Explode(Color color, float radius, Tilemap map, Action<Color, Vector3Int> callback, GameObject player)
        {
            PlayerDetails playerDetails = player.GetComponent<PlayerDetails>();
            
            // explosion VFX TODO
            Bounds bounds = new Bounds(transform.position, new Vector3(radius*2+1,radius*2+1,1));
            bounds = GameManager.Instance.ZoneManager.ConvertBoundsToTilemapBounds(bounds);
            GameManager.Instance.ZoneManager.ColorAllTiles(map, bounds, color, callback);
            
            RaycastHit2D[] objects = Physics2D.BoxCastAll((Vector2)(transform.position), new Vector2(radius, radius), 0, Vector2.zero);
            
            foreach (RaycastHit2D t in objects)
            {
                if( t.transform.gameObject.layer != LayerMask.NameToLayer("Player")) continue;
                if (playerDetails.PlayerID == t.transform.gameObject.GetComponent<PlayerDetails>().PlayerID)
                    continue;
                Debug.Log(t.transform.gameObject.GetComponent<PlayerDetails>().PlayerID);
                PlayerController playerController =
                    t.transform.gameObject.GetComponent<PlayerController>();
                playerController.Respawn();
            }
            
            
            Destroy(gameObject);
        }
    }
}