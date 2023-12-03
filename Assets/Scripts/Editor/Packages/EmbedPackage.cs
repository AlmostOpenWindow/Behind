using System;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace Editor.Packages
{
    static class EmbedPackage
    {
        static String targetPackage;
        static EmbedRequest Request;
        static ListRequest LRequest;
        private static string packageName;
        
        public static void EmbedPackageByName(string name)
        {
            if (LRequest != null && !LRequest.IsCompleted)
                return;
            
            packageName = name;
            LRequest = Client.List();
            EditorApplication.update += LProgress;
        }

        static void LProgress()
        {
            if (LRequest.IsCompleted)
            {
                if (LRequest.Status == StatusCode.Success)
                {
                    foreach (var package in LRequest.Result)
                    {
                        if (package.name != packageName)
                            continue;
                        
                        Debug.Log("Project found: " + package.name + "\n Source: " + package.source);
                        
                        // Only retrieve packages that are currently installed in the
                        // project (and are neither Built-In nor already Embedded)
                        if (package.isDirectDependency && package.source
                            != PackageSource.BuiltIn && package.source
                            != PackageSource.Embedded)
                        {
                            targetPackage = package.name;
                            break;
                        }
                    }

                }
                else
                    Debug.Log(LRequest.Error.message);

                EditorApplication.update -= LProgress;

                Embed(targetPackage);

            }
        }

        static void Embed(string inTarget)
        {
            // Embed a package in the project
            Debug.Log("Embed('" + inTarget + "') called");
            Request = Client.Embed(inTarget);
            EditorApplication.update += Progress;

        }

        static void Progress()
        {
            if (Request.IsCompleted)
            {
                if (Request.Status == StatusCode.Success)
                    Debug.Log("Embedded: " + Request.Result.packageId);
                else if (Request.Status >= StatusCode.Failure)
                    Debug.Log(Request.Error.message);

                EditorApplication.update -= Progress;
            }
        }
    }
}
