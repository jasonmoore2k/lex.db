#if NETFX_CORE
using Windows.Storage;
#endif

namespace Lex.Db
{
  interface IDbStorage
  {
#if NETFX_CORE
    IDbSchemaStorage OpenSchema(string path, StorageFolder storageFolder);
#endif

    IDbSchemaStorage OpenSchema(string path);

    bool IncreaseQuotaTo(long quota);

    bool HasEnoughQuota(long quota);
  }
}
