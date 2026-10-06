using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.CodeEditor;
using UnityEditor;

namespace NeovimEditor
{
    public interface IDiscovery
    {
        CodeEditor.Installation[] Installations { get; }

        void AddInstallation(string editorPath);
    }

    public class NeovimDiscovery : IDiscovery
    {
        public static readonly string neovim_installations = "kNeovimInstallations";

        private List<CodeEditor.Installation> installations;

        public CodeEditor.Installation[] Installations
        {
            get
            {
                if (this.installations is null)
                {
                    this.InitializeInstallations();
                }

                return this.installations.ToArray();
            }
        }

        public CodeEditor.Installation[] PathCallback()
        {
            return this.Installations;
        }

        public void AddInstallation(string editorPath)
        {
            var installation = CreateInstallation(editorPath);
            this.installations.Add(installation);

            var installationsPref = EditorPrefs.GetString(neovim_installations, "");
            installationsPref += $"{editorPath};";
            EditorPrefs.SetString(neovim_installations, installationsPref);
        }

        private void InitializeInstallations()
        {
            var installations = new List<CodeEditor.Installation>()
            {
                new CodeEditor.Installation()
                {
                    Name = "nvim",
                    Path = "/usr/bin/nvim",
                }
            };

            if (EditorPrefs.HasKey(neovim_installations))
            {
                var neovimInstallations = EditorPrefs.GetString(neovim_installations)
                    .Split(';')
                    .Select(CreateInstallation);
                installations.AddRange(neovimInstallations);
            }
            this.installations = installations;
        }

        private static CodeEditor.Installation CreateInstallation(string editorPath)
        {
            var name = Path.GetFileName(editorPath);
            return new CodeEditor.Installation()
            {
                Name = $"Neovim ({name})",
                Path = editorPath,
            };
        }
    }
}

