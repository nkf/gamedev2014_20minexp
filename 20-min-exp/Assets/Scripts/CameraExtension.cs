using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

static class CameraExtension {

    public static void SwitchTo(this Camera c) {
        AudioListener al;
        foreach (var camera in Camera.allCameras.Where(camera => camera != c)) {
            camera.enabled = false;
            al = camera.gameObject.GetComponent<AudioListener>();
            if (al != null) al.enabled = false;
        }
        c.enabled = true;
        al = c.gameObject.GetComponent<AudioListener>();
        if(al != null) al.enabled = true;
    }
}
