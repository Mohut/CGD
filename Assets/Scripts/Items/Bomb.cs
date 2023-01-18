using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Items
{
    public class Bomb : MonoBehaviour
    {
        [SerializeField] private Material lineMaterial;
        public void Trigger(Color color, float radius, float timeToDetonate, Tilemap map, Action<Color, Vector3Int> callback, GameObject player)
        {
            gameObject.GetComponent<SpriteRenderer>().color = color;
            DrawCircle(radius, 0.3f,color);
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
        public void DrawCircle(float radius, float lineWidth, Color color)
        {
            radius += + 0.5f;
            
            Gradient gradient;
            GradientColorKey[] colorKey;
            GradientAlphaKey[] alphaKey;

            gradient = new Gradient();
            
            colorKey = new GradientColorKey[2];
            colorKey[0].color = color;
            colorKey[0].time = 0.0f;
            colorKey[1].color = color;
            colorKey[1].time = 1.0f;

            // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
            alphaKey = new GradientAlphaKey[2];
            alphaKey[0].alpha = 0.8f;
            alphaKey[0].time = 0.0f;
            alphaKey[1].alpha = 0.8f;
            alphaKey[1].time = 1.0f;

            gradient.SetKeys(colorKey, alphaKey);
            var segments = 360;
            var line = gameObject.AddComponent<LineRenderer>();
            line.material = lineMaterial;
            line.colorGradient = gradient;
            line.useWorldSpace = false;
            line.startWidth = lineWidth;
            line.endWidth = lineWidth;
            line.positionCount = segments + 1;

            var pointCount = segments + 1; // add extra point to make startpoint and endpoint the same to close the circle
            var points = new Vector3[pointCount];

            for (int i = 0; i < pointCount; i++)
            {
                var rad = Mathf.Deg2Rad * (i * 360f / segments);
                points[i] = new Vector3(Mathf.Sin(rad) * radius,  Mathf.Cos(rad) * radius, 0);
            }

            line.SetPositions(points);
        }
    }
}