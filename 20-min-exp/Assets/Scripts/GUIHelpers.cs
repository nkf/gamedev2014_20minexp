using UnityEngine;
using System.Collections;

public class GUIHelpers {

	public static void DrawQuad(Rect position, Color color) {
		Texture2D texture = new Texture2D(1, 1);
		texture.SetPixel(0,0,color);
		texture.Apply();
		GUI.skin.box.normal.background = texture;
		GUI.Box(position, GUIContent.none);
	}

	public static void DrawCrossHair(SelectionType mode)
	{
		int cursorSizeX = 5;
		int cursorSizeY = 5;
		Color col = Color.green;
		Color tra = new Color(0.0f, 0.0f, 0.0f, 0.0f);

		// Initialize crosshair texture
		Texture2D tex = new Texture2D(cursorSizeX, cursorSizeY);
		for (int x = 0; x < cursorSizeX; x++) {
			for (int y = 0; y < cursorSizeY; y++) {
				if (x == 2 || y == 2)
					tex.SetPixel(x, y, col);
				else
					tex.SetPixel(x, y, tra);
			}
		}
		tex.SetPixel(2, 2, col);
		tex.Apply();

		// Determine where to draw crosshair
		Rect finalPos = new Rect();
		switch(mode) {
		case SelectionType.MIDDLE_SCREEN:
			finalPos.Set(
				(Screen.width/2) - (cursorSizeX/2),
				(Screen.height/2) - (cursorSizeY/2),
				cursorSizeX,
				cursorSizeY
				);
			break;
		case SelectionType.MOUSE:
			finalPos.Set(
				Event.current.mousePosition.x - (cursorSizeX/2),
				Event.current.mousePosition.y - (cursorSizeY/2),
				cursorSizeX,
				cursorSizeY
				);
			break;
		}

		// Draw crosshair
		GUI.DrawTexture (finalPos, tex);
	}
}