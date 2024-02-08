﻿using System;
using System.Collections.Generic;
using System.Linq;
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

        public bool ConsiderObjectNamedButtonAsButtons = false;

        [Space(10)]
        [Tooltip("Scene used for prototype assets, including canvas")]
        public string RunTimeAssetsScenePath;

        [Tooltip("Setup the root path of Figma assets")]
        public string FigmaAssetRootPath = "Assets/Figma";

        [Tooltip("Setup the root path of Figma assets")]
        public string FigmaBackupAssetRootPath = "Assets/Editor/Figma/Backup";

        [Header("Custom paths for types")] 
        public bool UseCustomPathForScreen = false;

        [HideInInspector] public string CustomPathScreen;
        public bool UseCustomPathForComponent = false;
        [HideInInspector] public string CustomPathComponent;

        public bool UseCustomPathForPage = false;
        [HideInInspector] public string CustomPathPage;

        public bool UseCustomPathForFont = false;
        [HideInInspector] public string CustomPathFont;

        public bool UseCustomPathForServerRenderedImage = false;
        [HideInInspector] public string CustomPathServerRenderedImage;

        public bool UseCustomPathForImageFills = false;
        [HideInInspector] public string CustomPathImageFills;
        
        
        [Space]
        [Tooltip("Enable Auto layout components (Horizontal/Vertical layout) (EXPERIMENTAL)")]
        public bool EnableAutoLayout = false;
        
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

        [Tooltip("If true, update existing screens prefabs")]
        public bool UpdateExistingPrefab = false;
        
        [Tooltip("If true, keep all child objects in screens prefabs")]
        public bool KeepScreenPrefabChildren = false;

        [Tooltip("If true the pivot and the anchor will be the same as the one set in Figma")]
        public bool MergeAnchorAndPivot = true;
        
        
        [Header("TextMeshPro FigmaText settings")]
        [Tooltip("These settings are here as Figma text handling differs slightly from TMP")]
        public float SpaceBetweenCharacters = -2.5f;
        public float TextMargins = -2;

        [Tooltip("If true this will use Kyub Emoji TMP instead of TextMeshPro")]
        public bool UseEmojiTMP = true;

        [Tooltip("If you are willing to use your custom Text class check this")]
        public bool UseCustomTextClass = true;

        [HideInInspector] public string TextTypeNamespace;
        [HideInInspector] public string TextTypeName;
        
        [HideInInspector]
        public List<FigmaPageData> PageDataList = new ();

        public string FileId {
            get
            {
                var (isValid, fileId) = FigmaApiUtils.GetFigmaDocumentIdFromUrl(DocumentUrl);
                return isValid ? fileId : "";
            }
        }
        
        public void RefreshForUpdatedPages(FigmaFile file)
        {
            // Get all pages from Figma Doc
            var pageNodeList = FigmaDataUtils.GetPageNodes(file);
            var downloadPageNodeIdList = pageNodeList.Select(p => p.id).ToList();

            // Get a list of all pages in the settings file
            var settingsPageDataIdList = PageDataList.Select(p => p.NodeId).ToList();

            // Build a list of all new pages to add
            var addPageIdList = downloadPageNodeIdList.Except(settingsPageDataIdList);
            foreach (var addPageId in addPageIdList)
            {
                var addNode = pageNodeList.FirstOrDefault(p => p.id == addPageId);
                PageDataList.Add(new FigmaPageData(addNode.name, addNode.id));
            }
            
            // Build a list of removed pages to remove from list
            var deletePageIdList = settingsPageDataIdList.Except(downloadPageNodeIdList);
            foreach (var deletePageId in deletePageIdList)
            {
                var index = PageDataList.FindIndex(p => p.NodeId == deletePageId);
                PageDataList.RemoveAt(index);
            }
            PageDataList.OrderBy(p => p.NodeId);
        }
    }

    [Serializable]
    public class FigmaPageData
    {
        public string Name;
        public string NodeId;
        public bool Selected;

        public FigmaPageData(){}

        public FigmaPageData(string name, string nodeId)
        {
            Name = name;
            NodeId = nodeId;
            Selected = true; // default is true
        }
    }
    
}