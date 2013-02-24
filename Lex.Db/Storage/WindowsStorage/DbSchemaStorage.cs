﻿#if NETFX_CORE
using System.Collections.Generic;
using Windows.Storage;

namespace Lex.Db.WindowsStorage
{
  class DbSchemaStorage : IDbSchemaStorage
  {
    readonly StorageFolder _localData;
    readonly List<DbTableStorage> _tables = new List<DbTableStorage>();
    readonly string _path;

    public DbSchemaStorage(string path)
        : this(path, ApplicationData.Current.LocalFolder)
    {
    }

    public DbSchemaStorage(string path, StorageFolder localData)
    {
      _path = path;
      _localData = localData;
    }

    public string Path { get { return _path; } }

    StorageFolder _folder;

    void Open(Awaiter awaiter)
    {
      _folder = awaiter.Await(_localData.CreateFolderAsync(_path, CreationCollisionOption.OpenIfExists));
    }

    public void Open()
    {
      using (var awaiter = new Awaiter(true))
        Open(awaiter);
    }

    void Purge(Awaiter awaiter)
    {
      awaiter.Await(_folder.DeleteAsync(StorageDeleteOption.PermanentDelete));
    }

    public void Purge()
    {
      using (var awaiter = new Awaiter(true))
      {
        Purge(awaiter);
        Open(awaiter);

        foreach (var i in _tables)
          i.Open(awaiter);
      }
    }

    public IDbTableStorage GetTable(string name)
    {
      var result = new DbTableStorage(_folder, name);
      _tables.Add(result);
      return result;
    }
  }
}
#endif