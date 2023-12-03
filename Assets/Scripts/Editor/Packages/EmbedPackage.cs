using System;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace Editor.Packages
{
    static class EmbedPackage
    {
        private static string _targetPackage;
        private static EmbedRequest _request;
        private static ListRequest _lRequest;
        private static string _packageName;
        
        public static void EmbedPackageByName(string name)
        {
            if (_lRequest != null && !_lRequest.IsCompleted)
                return;
            
            _packageName = name;
            _lRequest = Client.List();
            EditorApplication.update += LProgress;
        }

        static void LProgress()
        {
            if (_lRequest.IsCompleted)
            {
                if (_lRequest.Status == StatusCode.Success)
                {
                    foreach (var package in _lRequest.Result)
                    {
                        if (package.name != _packageName)
                            continue;
                        
                        Debug.Log("Project found: " + package.name + "\n Source: " + package.source);
                        
                        // Only retrieve packages that are currently installed in the
                        // project (and are neither Built-In nor already Embedded)
                        if (package.isDirectDependency && package.source
                            != PackageSource.Embedded)
                        {
                            _targetPackage = package.name;
                            break;
                        }
                    }

                }
                else
                    Debug.Log(_lRequest.Error.message);

                EditorApplication.update -= LProgress;

                if (string.IsNullOrEmpty(_targetPackage))
                {
                    Debug.Log("Unappropriated package source or wrong name: " + _packageName);
                    return;
                }
                
                Embed(_targetPackage);

            }
        }

        static void Embed(string inTarget)
        {
            // Embed a package in the project
            Debug.Log("Embed('" + inTarget + "') called");
            _request = Client.Embed(inTarget);
            EditorApplication.update += Progress;

        }

        static void Progress()
        {
            if (_request.IsCompleted)
            {
                if (_request.Status == StatusCode.Success)
                    Debug.Log("Embedded: " + _request.Result.packageId);
                else if (_request.Status >= StatusCode.Failure)
                    Debug.Log(_request.Error.message);

                EditorApplication.update -= Progress;
            }
        }
    }
}
