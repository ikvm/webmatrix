namespace Microsoft.Matrix.Utility
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Resources;

    public sealed class Library
    {
        private Icon _glyph;
        private IDictionary _metaData;
        private Image _preview;
        private IDictionary _sections;
        public static string ClientFilesSection = "ClientFiles";
        public static string DesignTimeFilesSection = "DesignTimeFiles";
        private const string GlyphResourceName = "MCLGlyph";
        private static System.Version libraryVersion = new System.Version(1, 0);
        private const string MetaDataResourceName = "MCLMetaData";
        private const string PreviewResourceName = "MCLPreview";
        public static string RuntimeFilesSection = "RuntimeFiles";
        private const string SectionsResourceName = "MCLSections";
        public static string SourceFilesSection = "SourceFiles";

        private Library()
        {
        }

        private Library(Stream metaDataStream, Stream glyphStream, Stream previewStream, Stream sectionsStream)
        {
            IDictionaryEnumerator enumerator;
            ResourceReader reader = null;
            try
            {
                IDictionary metaData = this.MetaData;
                reader = new ResourceReader(metaDataStream);
                enumerator = reader.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    DictionaryEntry current = (DictionaryEntry) enumerator.Current;
                    metaData[current.Key] = current.Value;
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            if (glyphStream != null)
            {
                ResourceReader reader2 = null;
                try
                {
                    reader2 = new ResourceReader(glyphStream);
                    enumerator = reader2.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        DictionaryEntry entry2 = (DictionaryEntry) enumerator.Current;
                        byte[] buffer = (byte[]) entry2.Value;
                        if ((buffer != null) && (buffer.Length > 0))
                        {
                            this._glyph = new Icon(new MemoryStream(buffer));
                        }
                    }
                }
                finally
                {
                    if (reader2 != null)
                    {
                        reader2.Close();
                    }
                }
            }
            if (previewStream != null)
            {
                ResourceReader reader3 = null;
                try
                {
                    reader3 = new ResourceReader(previewStream);
                    enumerator = reader3.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        DictionaryEntry entry3 = (DictionaryEntry) enumerator.Current;
                        byte[] buffer2 = (byte[]) entry3.Value;
                        if ((buffer2 != null) && (buffer2.Length > 0))
                        {
                            this._preview = Image.FromStream(new MemoryStream(buffer2));
                        }
                    }
                }
                finally
                {
                    if (reader3 != null)
                    {
                        reader3.Close();
                    }
                }
            }
            ResourceReader reader4 = null;
            try
            {
                reader4 = new ResourceReader(sectionsStream);
                enumerator = reader4.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    DictionaryEntry entry4 = (DictionaryEntry) enumerator.Current;
                    string key = (string) entry4.Key;
                    IDictionary section = this.GetSection(key);
                    byte[] buffer3 = (byte[]) entry4.Value;
                    ResourceReader reader5 = null;
                    try
                    {
                        reader5 = new ResourceReader(new MemoryStream(buffer3));
                        IDictionaryEnumerator enumerator2 = reader5.GetEnumerator();
                        while (enumerator2.MoveNext())
                        {
                            DictionaryEntry entry5 = (DictionaryEntry) enumerator2.Current;
                            section[(string) entry5.Key] = entry5.Value;
                        }
                        continue;
                    }
                    finally
                    {
                        if (reader5 != null)
                        {
                            reader5.Close();
                        }
                    }
                }
            }
            finally
            {
                if (reader4 != null)
                {
                    reader4.Close();
                }
            }
        }

        public static Library CreateLibrary(byte[] rawBytes)
        {
            if ((rawBytes == null) || (rawBytes.Length == 0))
            {
                throw new ArgumentNullException("rawBytes");
            }
            Library library = null;
            AppDomain domain = null;
            AssemblyResourceLoader loader = null;
            try
            {
                domain = AppDomain.CreateDomain("AssemblyResourceLoader AppDomain", AppDomain.CurrentDomain.Evidence, AppDomain.CurrentDomain.SetupInformation);
                Type type = typeof(AssemblyResourceLoader);
                loader = (AssemblyResourceLoader) domain.CreateInstanceAndUnwrap(type.Assembly.GetName().FullName, type.FullName);
                if (loader == null)
                {
                    return library;
                }
                loader.Initialize(rawBytes);
                byte[] resource = loader.GetResource("MCLMetaData");
                byte[] buffer = loader.GetResource("MCLGlyph");
                byte[] buffer3 = loader.GetResource("MCLPreview");
                byte[] buffer4 = loader.GetResource("MCLSections");
                if ((resource == null) || (buffer4 == null))
                {
                    throw new ApplicationException("Invalid Managed Component Library");
                }
                MemoryStream metaDataStream = null;
                MemoryStream glyphStream = null;
                MemoryStream sectionsStream = null;
                MemoryStream previewStream = null;
                try
                {
                    metaDataStream = new MemoryStream(resource);
                    if (buffer != null)
                    {
                        glyphStream = new MemoryStream(buffer);
                    }
                    if (buffer3 != null)
                    {
                        previewStream = new MemoryStream(buffer3);
                    }
                    sectionsStream = new MemoryStream(buffer4);
                    return new Library(metaDataStream, glyphStream, previewStream, sectionsStream);
                }
                finally
                {
                    if (metaDataStream != null)
                    {
                        metaDataStream.Close();
                    }
                    if (glyphStream != null)
                    {
                        glyphStream.Close();
                    }
                    if (previewStream != null)
                    {
                        previewStream.Close();
                    }
                    if (sectionsStream != null)
                    {
                        sectionsStream.Close();
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (loader != null)
                {
                    loader = null;
                }
                if (domain != null)
                {
                    AppDomain.Unload(domain);
                    domain = null;
                }
            }
            return library;
        }

        public static Library CreateLibrary(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentException("assembly");
            }
            Stream metaDataStream = null;
            Stream glyphStream = null;
            Stream previewStream = null;
            Stream sectionsStream = null;
            Library library = null;
            try
            {
                metaDataStream = assembly.GetManifestResourceStream("MCLMetaData");
                glyphStream = assembly.GetManifestResourceStream("MCLGlyph");
                previewStream = assembly.GetManifestResourceStream("MCLPreview");
                sectionsStream = assembly.GetManifestResourceStream("MCLSections");
                if ((metaDataStream == null) || (sectionsStream == null))
                {
                    throw new ApplicationException("Invalid Managed Component Library");
                }
                library = new Library(metaDataStream, glyphStream, previewStream, sectionsStream);
            }
            finally
            {
                if (metaDataStream != null)
                {
                    metaDataStream.Close();
                }
                if (glyphStream != null)
                {
                    glyphStream.Close();
                }
                if (previewStream != null)
                {
                    previewStream.Close();
                }
                if (sectionsStream != null)
                {
                    sectionsStream.Close();
                }
            }
            return library;
        }

        public static Library CreateLibrary(string author, string email, string libraryName, System.Version version, Icon glyph)
        {
            if ((author == null) || (author.Length == 0))
            {
                throw new ArgumentException("author");
            }
            if ((email == null) || (email.Length == 0))
            {
                throw new ArgumentException("email");
            }
            if ((libraryName == null) || (libraryName.Length == 0))
            {
                throw new ArgumentException("name");
            }
            if (version == null)
            {
                throw new ArgumentException("version");
            }
            Library library = new Library();
            library.MetaData["Author"] = author;
            library.MetaData["Email"] = email;
            library.MetaData["Name"] = libraryName;
            library.MetaData["Version"] = version.ToString();
            library.Glyph = glyph;
            return library;
        }

        public IDictionary GetSection(string sectionName)
        {
            IDictionary dictionary = this.Sections[sectionName] as IDictionary;
            if (dictionary == null)
            {
                dictionary = new Hashtable();
                this.Sections[sectionName] = dictionary;
            }
            return dictionary;
        }

        private void PackFile(IDictionary section, string source, string destination)
        {
            FileStream stream = null;
            try
            {
                stream = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.Read);
                int length = (int) stream.Length;
                byte[] buffer = new byte[length];
                stream.Read(buffer, 0, length);
                section[destination] = buffer;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream = null;
                }
            }
        }

        private void PackFile(string sectionName, string source, string destination)
        {
            this.PackFile(this.GetSection(sectionName), source, destination);
        }

        public void PackFiles(string sectionName, string sourceDirectory)
        {
            if ((sectionName == null) || (sectionName.Length == 0))
            {
                throw new ArgumentNullException("sectionName");
            }
            if ((sourceDirectory == null) || (sourceDirectory.Length == 0))
            {
                throw new ArgumentNullException("sourceDirectory");
            }
            if (!Directory.Exists(sourceDirectory))
            {
                throw new ArgumentException("sourceDirectory");
            }
            DirectoryInfo directory = new DirectoryInfo(sourceDirectory);
            this.PackFilesHelper(this.GetSection(sectionName), directory, string.Empty);
        }

        private void PackFilesHelper(IDictionary section, DirectoryInfo directory, string destination)
        {
            foreach (FileInfo info in directory.GetFiles())
            {
                string str = Path.Combine(destination, info.Name);
                this.PackFile(section, info.FullName, str);
            }
            foreach (DirectoryInfo info2 in directory.GetDirectories())
            {
                this.PackFilesHelper(section, info2, Path.Combine(destination, info2.Name));
            }
        }

        public void Save(string libraryFileName)
        {
            if ((libraryFileName == null) || (libraryFileName.Length == 0))
            {
                throw new ArgumentNullException("libraryFileName");
            }
            string fileName = Path.GetFileName(libraryFileName);
            AssemblyName name = new AssemblyName();
            name.Name = fileName;
            name.Version = new System.Version(this.Version);
            AssemblyBuilder builder = AppDomain.CurrentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.Save);
            ModuleBuilder builder2 = builder.DefineDynamicModule("mcl.resources", fileName);
            IResourceWriter writer = null;
            IResourceWriter writer2 = null;
            IResourceWriter writer3 = null;
            IResourceWriter writer4 = null;
            try
            {
                writer = builder2.DefineResource("MCLMetaData", "Managed Component Library MetaData", ResourceAttributes.Public);
                IDictionaryEnumerator enumerator = this.MetaData.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    DictionaryEntry current = (DictionaryEntry) enumerator.Current;
                    string key = (string) current.Key;
                    string str3 = (string) current.Value;
                    if (str3 == null)
                    {
                        str3 = string.Empty;
                    }
                    writer.AddResource(key, str3);
                }
                writer2 = builder2.DefineResource("MCLGlyph", "Managed Component Library Glyph", ResourceAttributes.Public);
                if (this._glyph != null)
                {
                    MemoryStream outputStream = new MemoryStream();
                    this._glyph.Save(outputStream);
                    writer2.AddResource("glyph", outputStream.ToArray());
                }
                writer3 = builder2.DefineResource("MCLPreview", "Managed Component Library Preview", ResourceAttributes.Public);
                if (this._preview != null)
                {
                    MemoryStream stream = new MemoryStream();
                    this._preview.Save(stream, ImageFormat.Jpeg);
                    writer3.AddResource("preview", stream.ToArray());
                }
                writer4 = builder2.DefineResource("MCLSections", "Managed Component Library Sections", ResourceAttributes.Public);
                enumerator = this.Sections.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    DictionaryEntry entry2 = (DictionaryEntry) enumerator.Current;
                    MemoryStream stream3 = new MemoryStream();
                    ResourceWriter writer5 = null;
                    try
                    {
                        writer5 = new ResourceWriter(stream3);
                        IDictionaryEnumerator enumerator2 = ((IDictionary) entry2.Value).GetEnumerator();
                        while (enumerator2.MoveNext())
                        {
                            DictionaryEntry entry3 = (DictionaryEntry) enumerator2.Current;
                            writer5.AddResource((string) entry3.Key, (byte[]) entry3.Value);
                        }
                        writer5.Generate();
                    }
                    finally
                    {
                        writer5.Close();
                    }
                    writer4.AddResource((string) entry2.Key, stream3.ToArray());
                }
                ConstructorInfo constructor = typeof(AssemblyVersionAttribute).GetConstructor(new Type[] { typeof(string) });
                builder.SetCustomAttribute(new CustomAttributeBuilder(constructor, new object[] { libraryVersion.ToString() }));
                ConstructorInfo con = typeof(AssemblyDescriptionAttribute).GetConstructor(new Type[] { typeof(string) });
                builder.SetCustomAttribute(new CustomAttributeBuilder(con, new object[] { "Managed Component Library" }));
                ConstructorInfo info3 = typeof(AssemblyTitleAttribute).GetConstructor(new Type[] { typeof(string) });
                builder.SetCustomAttribute(new CustomAttributeBuilder(info3, new object[] { Path.GetFileNameWithoutExtension(libraryFileName) }));
                builder.Save(fileName);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
                if (writer2 != null)
                {
                    writer2.Close();
                }
                if (writer4 != null)
                {
                    writer4.Close();
                }
            }
        }

        public ICollection UnpackFiles(string sectionName, string destinationDirectory)
        {
            if ((sectionName == null) || (sectionName.Length == 0))
            {
                throw new ArgumentNullException("sectionName");
            }
            if ((destinationDirectory == null) || (destinationDirectory.Length == 0))
            {
                throw new ArgumentNullException("destinationDirectory");
            }
            if (!Directory.Exists(destinationDirectory))
            {
                throw new ArgumentException("destinationDirectory");
            }
            IDictionary dictionary = this.Sections[sectionName] as IDictionary;
            if (dictionary == null)
            {
                return new ArrayList();
            }
            ArrayList list = new ArrayList();
            IDictionaryEnumerator enumerator = dictionary.GetEnumerator();
            while (enumerator.MoveNext())
            {
                DictionaryEntry current = (DictionaryEntry) enumerator.Current;
                string path = Path.Combine(destinationDirectory, (string) current.Key);
                string directoryName = Path.GetDirectoryName(path);
                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }
                FileStream stream = null;
                try
                {
                    stream = new FileStream(path, FileMode.Create);
                    byte[] buffer = (byte[]) current.Value;
                    stream.Write(buffer, 0, buffer.Length);
                    stream.Flush();
                    list.Add(path);
                    continue;
                }
                finally
                {
                    if (stream != null)
                    {
                        stream.Close();
                    }
                }
            }
            return list;
        }

        public string Author
        {
            get
            {
                return (string) this.MetaData["Author"];
            }
        }

        public string Email
        {
            get
            {
                return (string) this.MetaData["Email"];
            }
        }

        public Icon Glyph
        {
            get
            {
                return this._glyph;
            }
            set
            {
                this._glyph = value;
            }
        }

        public IDictionary MetaData
        {
            get
            {
                if (this._metaData == null)
                {
                    this._metaData = new Hashtable(new CaseInsensitiveHashCodeProvider(CultureInfo.InvariantCulture), new CaseInsensitiveComparer(CultureInfo.InvariantCulture));
                }
                return this._metaData;
            }
        }

        public string Name
        {
            get
            {
                return (string) this.MetaData["Name"];
            }
        }

        public Image Preview
        {
            get
            {
                return this._preview;
            }
            set
            {
                this._preview = value;
            }
        }

        public IDictionary Sections
        {
            get
            {
                if (this._sections == null)
                {
                    this._sections = new Hashtable(new CaseInsensitiveHashCodeProvider(CultureInfo.InvariantCulture), new CaseInsensitiveComparer(CultureInfo.InvariantCulture));
                }
                return this._sections;
            }
        }

        public string Version
        {
            get
            {
                return (string) this.MetaData["Version"];
            }
        }

        public sealed class AssemblyResourceLoader : MarshalByRefObject
        {
            private Assembly _assembly;

            public byte[] GetResource(string name)
            {
                if (this._assembly == null)
                {
                    return null;
                }
                Stream manifestResourceStream = null;
                byte[] buffer = null;
                try
                {
                    manifestResourceStream = this._assembly.GetManifestResourceStream(name);
                    if (manifestResourceStream != null)
                    {
                        buffer = new byte[(int) manifestResourceStream.Length];
                        manifestResourceStream.Read(buffer, 0, buffer.Length);
                    }
                }
                finally
                {
                    if (manifestResourceStream != null)
                    {
                        manifestResourceStream.Close();
                        manifestResourceStream = null;
                    }
                }
                return buffer;
            }

            public void Initialize(byte[] assemblyBytes)
            {
                this._assembly = Assembly.Load(assemblyBytes);
            }

            public void Initialize(Assembly assembly)
            {
                this._assembly = assembly;
            }
        }
    }
}

