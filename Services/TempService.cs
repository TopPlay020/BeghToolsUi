using System.IO;

namespace BeghToolsUi.Services
{
    public class TempService : ISingletonable
    {
        public readonly string TempDir = Path.Combine(Path.GetTempPath(), "BeghToolsUI");
        public TempService() {
            Directory.CreateDirectory(TempDir);
            ClearTempFolder();
        }
        public void ClearTempFolder()
        {
            if (Directory.Exists(TempDir))
            {
                DirectoryInfo dir = new DirectoryInfo(TempDir);
                foreach (FileInfo file in dir.GetFiles())
                    file.Delete();
                foreach (DirectoryInfo subDir in dir.GetDirectories())
                    subDir.Delete(true);
            }
        }
    }
}
