using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using Unity.CodeEditor;

namespace NeovimEditor
{
	[InitializeOnLoad]
	public class NeovimScriptEditor : IExternalCodeEditor
	{
		const string neovim_argument = "neovim_arguments";
		const string neovim_extension = "neovim_userExtensions";
		const string use_custom_neovim = "neovim_custom";
		const string neovim_path = "neovim_path";


		private static readonly GUIContent k_ResetArguments = EditorGUIUtility.TrTextContent("Reset argument");
		private static readonly GUIContent k_AddInstallation = EditorGUIUtility.TrTextContent("Add Installation");
		private static readonly string pidRelativeFilePath = "./Library/neovim.pid";

		private string m_Arguments;
		private readonly IDiscovery m_Discoverability;
		private readonly IGenerator m_ProjectGeneration;


		private static readonly string[] k_SupportedFileNames = 
		{ 
			"neovide.exe",
		};
		private static bool IsOSX => Application.platform == RuntimePlatform.OSXEditor;
		private static string DefaultApp => EditorPrefs.GetString("kScriptsDefaultApp");
		private static string DefaultArgument { get; } = "\"$(File)\"";


		private string Arguments
		{
			get => m_Arguments ?? (m_Arguments = EditorPrefs.GetString(neovim_argument, DefaultArgument));
			set
			{
				m_Arguments = value;
				EditorPrefs.SetString(neovim_argument, value);
			}
		}

		private static int? NeovimPid
		{
			get => File.Exists(pidRelativeFilePath) ? int.Parse(File.ReadAllText(pidRelativeFilePath)) : (int?)null;
			set
			{
				var serializedValue = value != null ? value.Value.ToString() : string.Empty;
				File.WriteAllText(pidRelativeFilePath, serializedValue);
			}
		}

		private static string[] defaultExtensions
		{
			get
			{
				var customExtensions = new[] { "json", "asmdef", "log" };
				return EditorSettings.projectGenerationBuiltinExtensions
					.Concat(EditorSettings.projectGenerationUserExtensions)
					.Concat(customExtensions)
					.Distinct().ToArray();
			}
		}

		private static string[] HandledExtensions
		{
			get
			{
				return HandledExtensionsString
					.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
					.Select(s => s.TrimStart('.', '*'))
					.ToArray();
			}
		}

		private static string HandledExtensionsString
		{
			get => EditorPrefs.GetString(neovim_extension, string.Join(";", defaultExtensions));
			set => EditorPrefs.SetString(neovim_extension, value);
		}

		public CodeEditor.Installation[] Installations => this.m_Discoverability.Installations;


		public NeovimScriptEditor(IDiscovery discovery, IGenerator projectGeneration)
		{
			m_Discoverability = discovery;
			m_ProjectGeneration = projectGeneration;
		}

		static NeovimScriptEditor()
		{
			var editor = new NeovimScriptEditor(new NeovimDiscovery(), new ProjectGeneration(Directory.GetParent(Application.dataPath).FullName));
			CodeEditor.Register(editor);

			editor.CreateIfDoesntExist();
		}


		public bool TryGetInstallationForPath(string editorPath, out CodeEditor.Installation installation)
		{
			var lowerCasePath = editorPath.ToLower();
			var filename = Path.GetFileName(lowerCasePath).Replace(" ", "");
			if (!filename.StartsWith("nvim") && !k_SupportedFileNames.Contains(filename))
			{
				installation = default;
				return false;
			}

			var installations = m_Discoverability.Installations;
			if (!installations.Any())
			{
				installation = new CodeEditor.Installation
				{
					Name = $"Neovim ({filename})",
					Path = editorPath,
				};
			}
			else
			{
				try
				{
					installation = installations.First(inst => inst.Path == editorPath);
				}
				catch (InvalidOperationException)
				{
					installation = new CodeEditor.Installation
					{
						Name = $"Neovim ({filename})",
						Path = editorPath,
					};
				}
			}

			return true;
		}

		public void OnGUI()
		{
			if (GUILayout.Button("Add Installation", GUILayout.Width(120)))
			{
				string editorPath = EditorUtility.OpenFilePanel("Choose installation...", "", "exe");
				if (!string.IsNullOrEmpty(editorPath))
				{
					m_Discoverability.AddInstallation(editorPath);
				}
			}
			Arguments = EditorGUILayout.TextField("External Script Editor Args", Arguments);
			if (GUILayout.Button(k_ResetArguments, GUILayout.Width(120)))
			{
				Arguments = DefaultArgument;
			}

			EditorGUILayout.LabelField("Generate .csproj files for:");
			EditorGUI.indentLevel++;
			SettingsButton(ProjectGenerationFlag.Embedded, "Embedded packages", "");
			SettingsButton(ProjectGenerationFlag.Local, "Local packages", "");
			SettingsButton(ProjectGenerationFlag.Registry, "Registry packages", "");
			SettingsButton(ProjectGenerationFlag.Git, "Git packages", "");
			SettingsButton(ProjectGenerationFlag.BuiltIn, "Built-in packages", "");
#if UNITY_2019_3_OR_NEWER
			SettingsButton(ProjectGenerationFlag.LocalTarBall, "Local tarball", "");
#endif
			SettingsButton(ProjectGenerationFlag.Unknown, "Packages from unknown sources", "");
			RegenerateProjectFiles();
			EditorGUI.indentLevel--;

			HandledExtensionsString = EditorGUILayout.TextField(new GUIContent("Extensions handled: "), HandledExtensionsString);
		}

		private void RegenerateProjectFiles()
		{
			var rect = EditorGUI.IndentedRect(EditorGUILayout.GetControlRect(new GUILayoutOption[] { }));
			rect.width = 252;
			if (GUI.Button(rect, "Regenerate project files"))
			{
				m_ProjectGeneration.Sync();
			}
		}

		private void SettingsButton(ProjectGenerationFlag preference, string guiMessage, string toolTip)
		{
			var prevValue = m_ProjectGeneration.AssemblyNameProvider.ProjectGenerationFlag.HasFlag(preference);
			var newValue = EditorGUILayout.Toggle(new GUIContent(guiMessage, toolTip), prevValue);
			if (newValue != prevValue)
			{
				m_ProjectGeneration.AssemblyNameProvider.ToggleProjectGeneration(preference);
			}
		}

		public void CreateIfDoesntExist()
		{
			if (!m_ProjectGeneration.SolutionExists())
			{
				m_ProjectGeneration.Sync();
			}
		}

		public void SyncIfNeeded(string[] addedFiles, string[] deletedFiles, string[] movedFiles, string[] movedFromFiles, string[] importedFiles)
		{
			(m_ProjectGeneration.AssemblyNameProvider as IPackageInfoCache)?.ResetPackageInfoCache();
			m_ProjectGeneration.SyncIfNeeded(addedFiles.Union(deletedFiles).Union(movedFiles).Union(movedFromFiles).ToList(), importedFiles);
		}

		public void SyncAll()
		{
			(m_ProjectGeneration.AssemblyNameProvider as IPackageInfoCache)?.ResetPackageInfoCache();
			AssetDatabase.Refresh();
			m_ProjectGeneration.Sync();
		}

		public bool OpenProject(string path, int line, int column)
		{
			if (path != "" && (!SupportsExtension(path) || !File.Exists(path))) // Assets - Open C# Project passes empty path here
			{
				return false;
			}

			line = Math.Max(line, 1);
			column = Math.Max(column, 1);

			string arguments = GetArguments(path, line, column);
			var neovimPid = NeovimPid;
			if (neovimPid is null || !IsProcessRunning(neovimPid.Value))
			{
				return OpenNewInTerminal(arguments);
			}

			return OpenExistingInTerminal($"\\\\.\\pipe\\Unity-neovim.{neovimPid}.0", arguments);
		}

		private string GetArguments(string path, int line, int column)
		{
			string filePath = path.Length != 0 ? path : m_ProjectGeneration.ProjectDirectory;
			return $@"""{filePath}""";
		}

		private static bool OpenNewInTerminal(string arguments)
		{
			var pidsBefore = Process.GetProcessesByName("nvim").Select(x => x.Id).ToArray();
			using (var process = new Process())
			{
				process.StartInfo = new ProcessStartInfo()
				{
					FileName = DefaultApp,
					Arguments = $"-- --listen \"Unity-neovim\" --remote {arguments}",
					UseShellExecute = true,
				};
				process.Start();
			};

			NeovimPid = WaitForNvimPid(pidsBefore);
			return true;
		}

		private static int WaitForNvimPid(int[] existingPids)
		{
			int? id = null;
			while (id is null)
			{
				id = Process.GetProcessesByName("nvim")
					.Select(x => x.Id)
					.Except(existingPids)
					.Cast<int?>()
					.FirstOrDefault();
			}
			return id.Value;
		}

		private static bool OpenExistingInTerminal(string server, string arguments)
		{
			using (var process = new Process())
			{
				process.StartInfo = new ProcessStartInfo()
				{
					FileName = "nvim",
					Arguments = $@"--server {server} --remote {arguments}",
					CreateNoWindow = true,
					UseShellExecute = true,
					WindowStyle = ProcessWindowStyle.Hidden,
				};

				process.Start();
				process.Close();
			}
			return true;
		}

		private static bool SupportsExtension(string path)
		{
			var extension = Path.GetExtension(path);
			if (string.IsNullOrEmpty(extension))
				return false;
			return HandledExtensions.Contains(extension.TrimStart('.'));
		}

		public void Initialize(string editorInstallationPath) { }

		private static bool IsProcessRunning(int pid)
		{
			try
			{
				var process = Process.GetProcessById(pid);
				return !process.HasExited;
			}
			catch (ArgumentException)
			{
				return false;
			}
		}
	}
}

