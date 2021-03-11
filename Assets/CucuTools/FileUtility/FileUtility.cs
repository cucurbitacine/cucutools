using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CucuTools.FileUtility
{
    public class FileUtility
    {
        public static FileUtility Singleton { get; }

        static FileUtility()
        {
            Singleton = new FileUtility(nameof(Singleton), Encoding.Default);
        }
        
        public Encoding Encoding => _encoding;
        public string Name => _name;

        private Encoding _encoding;
        private string _name;

        public FileUtility(string name, Encoding encoding = null)
        {
            _name = name ?? "";
            _encoding = encoding ?? Encoding.Default;
        }

        public FileUtility(Encoding encoding) : this(encoding?.HeaderName, encoding)
        {
        }

        public bool Exists(string filePath)
        {
            return File.Exists(filePath);
        }
        
        #region CRUD
        
        public void Create(string filePath)
        {
            using var fs = new FileStream(filePath, FileMode.CreateNew);
        }

        public void Create(string filePath, byte[] content)
        {
            using var fs = new FileStream(filePath, FileMode.CreateNew);
            fs.Write(content, 0, content.Length);
        }
        
        public void Create(string filePath, string content)
        {
            Create(filePath, Encoding.GetBytes(content));
        }
        
        public async Task CreateAsync(string filePath, byte[] content)
        {
            using var fs = new FileStream(filePath, FileMode.CreateNew);
            await fs.WriteAsync(content, 0, content.Length);
        }
        
        public async Task CreateAsync(string filePath, string content)
        {
            await CreateAsync(filePath, Encoding.GetBytes(content));
        }

        public async Task<byte[]> ReadAsync(string filePath)
        {
            using var fs = new FileStream(filePath, FileMode.Open);
            var bytes = new byte[fs.Length];
            await fs.ReadAsync(bytes, 0, bytes.Length);
            return bytes;
        }

        public async Task<string> ReadStringAsync(string filePath)
        {
            return Encoding.GetString(await ReadAsync(filePath));
        }

        public byte[] Read(string filePath)
        {
            using var fs = new FileStream(filePath, FileMode.Open);
            var bytes = new byte[fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            return bytes;
        }

        public string ReadString(string filePath)
        {
            return Encoding.GetString(Read(filePath));
        }
        
        public async Task WriteAsync(string filePath, byte[] content)
        {
            using var fs = new FileStream(filePath, FileMode.OpenOrCreate);
            await fs.WriteAsync(content, 0, content.Length);
        }

        public async Task WriteAsync(string filePath, string content)
        {
            await WriteAsync(filePath, Encoding.GetBytes(content));
        }

        public void Write(string filePath, byte[] content)
        {
            using var fs = new FileStream(filePath, FileMode.OpenOrCreate);
            fs.Write(content, 0, content.Length);
        }

        public void Write(string filePath, string content)
        {
            Write(filePath, Encoding.GetBytes(content));
        }
        
        public async Task AppendAsync(string filePath, byte[] content)
        {
            using var fs = new FileStream(filePath, FileMode.Append);
            await fs.WriteAsync(content, 0, content.Length);
        }

        public async Task AppendAsync(string filePath, string content)
        {
            await AppendAsync(filePath, Encoding.GetBytes(content));
        }

        public void Append(string filePath, byte[] content)
        {
            using var fs = new FileStream(filePath, FileMode.Append);
            fs.Write(content, 0, content.Length);
        }

        public void Append(string filePath, string content)
        {
            Append(filePath, Encoding.GetBytes(content));
        }
        
        public void Delete(string filePath)
        {
            File.Delete(filePath);
        }
        
        #endregion
    }
    
    public static class FileUtilityExt
    {
        public static string FileExt(this string fileName, string ext)
        {
            if (!ext.StartsWith(".")) ext = $".{ext}";

            if (!fileName.EndsWith(ext)) fileName = $"{fileName}{ext}";

            return fileName;
        }
    }
}