using UnityEngine;
namespace TinyLocalization {
    public static class EditorHelper {
        public static Texture2D MakeTex(int width, int height, Color col) {
            Color[] pix = new Color[width*height];
    
            for(int i = 0; i < pix.Length; i++)
                pix[i] = col;
    
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
    
            return result;
        }
        
        public static GUIStyle MakeBackgroundStyle(Color color){
            GUIStyle style = new GUIStyle();
            style.normal.background = MakeTex(1, 1, color);
            
            return style;
        }
    }
}