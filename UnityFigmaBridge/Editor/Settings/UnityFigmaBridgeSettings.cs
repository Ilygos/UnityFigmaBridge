﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityFigmaBridge.Editor.FigmaApi;

namespace UnityFigmaBridge.Editor.Settings
{
    public class UnityFigmaBridgeSettings : ScriptableObject
    {
       
        [Tooltip("The FIGMA Document URL to import")]
        public string DocumentUrl;
        
        [Tooltip("Generate logic and linking of screens based on FIGMA's 'Prototype' settings")]
        public bool BuildPrototypeFlow=true;
        
        [Space(10)]
        [Tooltip("Scene used for prototype assets, including canvas")]
        public string RunTimeAssetsScenePath;
        
        
        [Tooltip("C# Namespace filter for binding MonoBehaviours for screens. Use this to ensure it will only bind to MonoBehaviours in that namespace (eg specify 'MyGame.UI' to only bind MyGame.UI.PlayScreen node to 'PlayScreen')")]
        public string ScreenBindingNamespace="";
        
        [Tooltip("Scale for rendering server images")]
        public int ServerRenderImageScale=3;

        [Tooltip("Tick this to enable downloading missing fonts from Google Fonts")]
        public bool EnableGoogleFontsDownloads = true;

        [Tooltip("Generate a C# file containing all found screens")]
        public bool CreateScreenNameCSharpFile = false;
        
        [Tooltip("If false, the generator will not attempt to build any nodes marked for export")]
        public bool GenerateNodesMarkedForExport = true;

        [Tooltip("If true, download only selected pages and screens")]
        public bool OnlyImportSelectedPages = false;

        [HideInInspector] public List<LineData> PageDataList = new ();
        [HideInInspector] public List<LineData> ScreenDataList = new ();

        public string FileId {
            get
            {
                var (isValid, fileId) = FigmaApiUtils.GetFigmaDocumentIdFromUrl(DocumentUrl);
                return isValid ? fileId : "";
            }
        }
    }

    [Serializable]
    public class LineData
    {
        public string Id;
        public bool IsChecked;

        public LineData(){}

        public LineData(string name, string id)
        {
            Id = id;
            IsChecked = true; // default is true
        }
    }
}