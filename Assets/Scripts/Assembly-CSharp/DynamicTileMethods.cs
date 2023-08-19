using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Assembly_CSharp
{
    public class DynamicTileMethods
    {
        private static bool ParseStringBool(string value)
        {
            switch (value)
            {
                case "True":
                    return true;
                case "False":
                    return false;
                default:
                    return false;
            }
        }

        public static void DynamicWinchester(DataTile tile, LineRenderer renderer)
        {
            renderer.enabled = true;
            renderer.startWidth = 0.1f * InputHandler.Instance.grid.transform.localScale.x;
            renderer.loop = true;
            renderer.gameObject.layer = 5;
            var localScale = InputHandler.Instance.grid.transform.localScale;
            renderer.positionCount = 4;
            renderer.startColor = Color.white;
            renderer.colorGradient = new Gradient()
            {
                mode = GradientMode.Blend,
                colorKeys = new GradientColorKey[]
                {
                    new GradientColorKey() { color = Color.white * 2, time = 0},
                },
                alphaKeys = new GradientAlphaKey[]
                {
                    new GradientAlphaKey() { alpha =1, time = 0},
                }
            };
            Vector2 pain = new Vector2(0, 0);
            foreach (KeyValuePair<string, JToken> att in tile.data)
            {
                if (att.Key == "Tile Size X")
                {
                    pain.x = int.Parse(att.Value.ToString());
                }
                if (att.Key == "Tile Size Y")
                {
                    pain.y = int.Parse(att.Value.ToString());
                }
            }
            renderer.motionVectorGenerationMode = MotionVectorGenerationMode.Camera;
            Vector2 pos = GridMap.Instance.map.CellToWorld(tile.worldIntPosition);
            renderer.SetPosition(0, new Vector3(pos.x, pos.y, 20));
            renderer.SetPosition(1, new Vector3(pos.x + (pain.x * localScale.x), pos.y, 20));
            renderer.SetPosition(2, new Vector3(pos.x + (pain.x * localScale.x), pos.y + (pain.y * localScale.y), 20));
            renderer.SetPosition(3, new Vector3(pos.x , pos.y + (pain.y * localScale.y), 20));
        }
        public static void DynamicConveyors(DataTile tile, LineRenderer renderer)
        {
            bool vert = false;
            if (tile.name == "conveyor_belt_up") { vert = true; }

            renderer.startWidth = 0.1f * InputHandler.Instance.grid.transform.localScale.x;
            renderer.loop = true;
            renderer.enabled = true;
            Tilemap map = Manager.Instance.GetTilemap(TilemapHandler.MapType.Nodes).map;
            Vector3 p = new Vector3(map.transform.position.x, map.transform.position.y, map.transform.position.z + 2);
            renderer.transform.position = p;
            renderer.sortingLayerName = "Foreground";
            var localScale = InputHandler.Instance.grid.transform.localScale;
            renderer.positionCount = 4;
            Vector2 pain = new Vector2(0, 0);
            bool b = false;
            foreach (KeyValuePair<string, JToken> att in tile.data)
            {
                if (att.Key == "Tile Size X")
                {
                    pain.x = int.Parse(att.Value.ToString());
                }
                if (att.Key == "Tile Size Y")
                {
                    pain.y = int.Parse(att.Value.ToString());
                }
                if (att.Key == "Reversed")
                {
                    b = ParseStringBool(att.Value.ToString());//Debug.LogError(att.Value.ToString());
                }
            }
            renderer.startColor = b ? Color.yellow : Color.red;
            renderer.endColor = b ? Color.yellow : Color.red;
            Color color = !b ? Color.yellow : Color.red;

            renderer.colorGradient = new Gradient()
            {
                mode = GradientMode.Blend,
                colorKeys = new GradientColorKey[]
                {
                    new GradientColorKey() { color = renderer.startColor * 2, time = 0 + (vert ? 0.25f : 0)},
                    new GradientColorKey() { color = color * 2, time = 0.25f + (vert ? 0.25f : 0)},
                    new GradientColorKey() { color = color * 2, time = 0.50f + (vert ? 0.25f : 0)},
                    new GradientColorKey() { color =  renderer.startColor * 2, time = 0.75f + (vert ? 0.25f : 0)}
                },
                alphaKeys = new GradientAlphaKey[]
                {
                    new GradientAlphaKey() { alpha =1, time = 0},
                }
            };

            var t = GridMap.Instance.map.CellToWorld(tile.worldIntPosition);
            Vector3 pos = new Vector3(t.x, t.y, map.transform.position.z -3 );
            Vector3 pos1 = new Vector3(pain.x * localScale.x, 0, map.transform.position.z - 3);
            Vector3 pos2 = new Vector3(pain.x * localScale.x, pain.y * localScale.y, map.transform.position.z - 3);
            Vector3 pos3 = new Vector3(0, pain.y * localScale.y, map.transform.position.z -3);


            renderer.SetPosition(0, pos);//new Vector3(pos.x, pos.y, pos.z));
            renderer.SetPosition(1, pos + pos1);//new Vector3(pos.x + (pain.x * localScale.x), pos.y, pos.z));
            renderer.SetPosition(2, pos + pos2);//new Vector3(pos.x + (pain.x * localScale.x), pos.y + (pain.y * localScale.y), pos.z));
            renderer.SetPosition(3, pos + pos3);//new Vector3(pos.x, pos.y + (pain.y * localScale.y), pos.z));
        }
        public static void DynamicPews(DataTile tile, LineRenderer renderer)
        {
            renderer.enabled = true;
            renderer.startWidth = 0.1f * InputHandler.Instance.grid.transform.localScale.x;
            renderer.loop = true;
            renderer.gameObject.layer = 5;
            var localScale = InputHandler.Instance.grid.transform.localScale;
            renderer.positionCount = 4;
            renderer.startColor = Color.green;
            renderer.colorGradient = new Gradient()
            {
                mode = GradientMode.Blend,
                colorKeys = new GradientColorKey[]
                {
                    new GradientColorKey() { color = Color.green * 2, time = 0},
                },
                alphaKeys = new GradientAlphaKey[]
                {
                    new GradientAlphaKey() { alpha =1, time = 0},
                }
            };
            Vector2 pain = new Vector2(0, 0);
            foreach (KeyValuePair<string, JToken> att in tile.data)
            {
                if (att.Key == "Pew Length")
                {
                    pain.x = int.Parse(att.Value.ToString());
                }
            }
            renderer.motionVectorGenerationMode = MotionVectorGenerationMode.Camera;
            Vector2 pos = GridMap.Instance.map.CellToWorld(tile.worldIntPosition);
            renderer.SetPosition(0, new Vector3(pos.x, pos.y, 20));
            renderer.SetPosition(1, new Vector3(pos.x + (pain.x * localScale.x), pos.y, 20));
            renderer.SetPosition(2, new Vector3(pos.x + (pain.x * localScale.x), pos.y + (pain.y * localScale.y), 20));
            renderer.SetPosition(3, new Vector3(pos.x, pos.y + (pain.y * localScale.y), 20));
        }

        public static void DynamicRollersHeight(DataTile tile, LineRenderer renderer)
        {
            renderer.enabled = true;
            renderer.startWidth = 0.1f * InputHandler.Instance.grid.transform.localScale.x;
            renderer.loop = true;
            renderer.gameObject.layer = 5;
            var localScale = InputHandler.Instance.grid.transform.localScale;
            renderer.positionCount = 4;
            renderer.startColor = Color.red;
            renderer.colorGradient = new Gradient()
            {
                mode = GradientMode.Blend,
                colorKeys = new GradientColorKey[]
                {
                    new GradientColorKey() { color = Color.red * 2, time = 0},
                },
                alphaKeys = new GradientAlphaKey[]
                {
                    new GradientAlphaKey() { alpha =1, time = 0},
                }
            };
            Vector2 pain = new Vector2(0, 0);
            foreach (KeyValuePair<string, JToken> att in tile.data)
            {
                if (att.Key == "Height")
                {
                    pain.y = int.Parse(att.Value.ToString());
                }
            }
            renderer.motionVectorGenerationMode = MotionVectorGenerationMode.Camera;
            Vector2 pos = GridMap.Instance.map.CellToWorld(tile.worldIntPosition);
            renderer.SetPosition(0, new Vector3(pos.x, pos.y, 20));
            renderer.SetPosition(1, new Vector3(pos.x + ((pain.x + 2.4375f) * localScale.x), pos.y, 20));
            renderer.SetPosition(2, new Vector3(pos.x + ((pain.x + 2.4375f) * localScale.x), pos.y + ((pain.y) * localScale.y), 20));
            renderer.SetPosition(3, new Vector3(pos.x, pos.y + (pain.y * localScale.y), 20));
        }

        public static void DynamicRollersLength(DataTile tile, LineRenderer renderer)
        {
            renderer.enabled = true;
            renderer.startWidth = 0.1f * InputHandler.Instance.grid.transform.localScale.x;
            renderer.loop = true;
            renderer.gameObject.layer = 5;
            var localScale = InputHandler.Instance.grid.transform.localScale;
            renderer.positionCount = 4;
            renderer.startColor = Color.red;
            renderer.colorGradient = new Gradient()
            {
                mode = GradientMode.Blend,
                colorKeys = new GradientColorKey[]
                {
                    new GradientColorKey() { color = Color.red * 2, time = 0},
                },
                alphaKeys = new GradientAlphaKey[]
                {
                    new GradientAlphaKey() { alpha =1, time = 0},
                }
            };
            Vector2 pain = new Vector2(0, 0);
            foreach (KeyValuePair<string, JToken> att in tile.data)
            {
                if (att.Key == "Length")
                {
                    pain.x = int.Parse(att.Value.ToString());
                }
            }
            renderer.motionVectorGenerationMode = MotionVectorGenerationMode.Camera;
            Vector2 pos = GridMap.Instance.map.CellToWorld(tile.worldIntPosition);
            renderer.SetPosition(0, new Vector3(pos.x, pos.y, 20));
            renderer.SetPosition(1, new Vector3(pos.x + ((pain.x) * localScale.x), pos.y, 20));
            renderer.SetPosition(2, new Vector3(pos.x + ((pain.x) * localScale.x), pos.y + ((pain.y + +2.4375f) * localScale.y), 20));
            renderer.SetPosition(3, new Vector3(pos.x, pos.y + (pain.y + +2.4375f * localScale.y), 20));
        }

        public static void DynamicMovingTiles(DataTile tile, LineRenderer renderer)
        {
            renderer.startWidth = 0.1f * InputHandler.Instance.grid.transform.localScale.x;
            renderer.loop = true;
            renderer.enabled = true;
            Tilemap map = Manager.Instance.GetTilemap(TilemapHandler.MapType.Nodes).map;
            Vector3 p = new Vector3(map.transform.position.x, map.transform.position.y, map.transform.position.z + 2);
            renderer.transform.position = p;
            renderer.sortingLayerName = "Foreground";
            var localScale = InputHandler.Instance.grid.transform.localScale;
            renderer.positionCount = 4;
            Vector2 pain = new Vector2(0, 0);
            bool b = false;
            foreach (KeyValuePair<string, JToken> att in tile.data)
            {
                if (att.Key == "Tile Size X")
                {
                    pain.x = int.Parse(att.Value.ToString());
                }
                if (att.Key == "Tile Size Y")
                {
                    pain.y = int.Parse(att.Value.ToString());
                }
            }
            renderer.startColor = Color.yellow;

            renderer.colorGradient = new Gradient()
            {
                mode = GradientMode.Blend,
                colorKeys = new GradientColorKey[]
                {
                    new GradientColorKey() { color = renderer.startColor * 2, time = 0 }
                },
                alphaKeys = new GradientAlphaKey[]
                {
                    new GradientAlphaKey() { alpha =1, time = 0},
                }
            };

            var t = GridMap.Instance.map.CellToWorld(tile.worldIntPosition);
            Vector3 pos = new Vector3(t.x, t.y, map.transform.position.z - 3);
            Vector3 pos1 = new Vector3(pain.x * localScale.x, 0, map.transform.position.z - 3);
            Vector3 pos2 = new Vector3(pain.x * localScale.x, pain.y * localScale.y, map.transform.position.z - 3);
            Vector3 pos3 = new Vector3(0, pain.y * localScale.y, map.transform.position.z - 3);


            renderer.SetPosition(0, pos);//new Vector3(pos.x, pos.y, pos.z));
            renderer.SetPosition(1, pos + pos1);//new Vector3(pos.x + (pain.x * localScale.x), pos.y, pos.z));
            renderer.SetPosition(2, pos + pos2);//new Vector3(pos.x + (pain.x * localScale.x), pos.y + (pain.y * localScale.y), pos.z));
            renderer.SetPosition(3, pos + pos3);//new Vector3(pos.x, pos.y + (pain.y * localScale.y), pos.z));
        }

    }
}
