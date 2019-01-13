using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Resources;
using Lunari.Tsuki.Exceptions;
using Shiroi.Cutscenes.Editor.Util;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Shiroi.Cutscenes.Editor {
    public static class ShiroiEditorUtil {
        public const string CSIcon = "dll Script Icon";

        public static Texture GetIconFor(Type type) {
            var tex = EditorGUIUtility.ObjectContent(null, type).image;
            if (tex == null) {
                tex = EditorGUIUtility.IconContent(CSIcon).image;
            }

            return tex;
        }

        /// <summary>
        /// Set the icon for this object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="texture">The icon.</param>
        public static void SetIcon(this Object obj, Texture2D texture) {
            var ty = typeof(EditorGUIUtility);
            var mi = ty.GetMethod("SetIconForObject", BindingFlags.NonPublic | BindingFlags.Static);
            mi.Invoke(null, new object[] {obj, texture});
        }

        /// <summary>
        /// Set the icon for this object from an embedded resource.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="texture">The icon.</param>
        public static void SetIcon(this Object obj, string resourceName) {
            SetIcon(obj, GetEmbeddedIcon(resourceName));
        }

        /// <summary>
        /// Read all bytes in this stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>All bytes in the stream.</returns>
        public static byte[] ReadAllBytes(this Stream stream) {
            long originalPosition = 0;

            if (stream.CanSeek) {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try {
                var readBuffer = new byte[4096];

                var totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0) {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead != readBuffer.Length) {
                        continue;
                    }

                    var nextByte = stream.ReadByte();
                    if (nextByte == -1) {
                        continue;
                    }

                    var temp = new byte[readBuffer.Length * 2];
                    Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                    Buffer.SetByte(temp, totalBytesRead, (byte) nextByte);
                    readBuffer = temp;
                    totalBytesRead++;
                }

                var buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead) {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }

                return buffer;
            } finally {
                if (stream.CanSeek) {
                    stream.Position = originalPosition;
                }
            }
        }

        private static readonly Dictionary<string, Texture2D> _embeddedIcons = new Dictionary<string, Texture2D>();

        /// <summary>
        /// Get the embedded icon with the given resource name.
        /// </summary>
        /// <param name="resourceName">The resource name.</param>
        /// <returns>The embedded icon with the given resource name.</returns>
        public static Texture2D GetEmbeddedIcon(string resourceName) {
            var assembly = Assembly.GetExecutingAssembly();

            Texture2D icon;
            try {
                if (!_embeddedIcons.TryGetValue(resourceName, out icon) || icon == null) {
                    byte[] iconBytes;
                    using (var stream = assembly.GetManifestResourceStream(resourceName)) {
                        iconBytes = stream.ReadAllBytes();
                    }

                    icon = new Texture2D(128, 128);
                    icon.LoadImage(iconBytes);
                    icon.name = resourceName;

                    _embeddedIcons[resourceName] = icon;
                }
            } catch (System.Exception e) {
                throw new WTFException(
                    $"Couldn't find image with resource name '{resourceName}', found resource names are: '{string.Join(",", assembly.GetManifestResourceNames())}'"
                );
            }

            return icon;
        }
    }
}