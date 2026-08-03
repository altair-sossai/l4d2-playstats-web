using L4D2PlayStats.Core.Contexts.AzureTableStorage;
using L4D2PlayStats.Core.Contexts.AzureTableStorage.Repositories;
using L4D2PlayStats.Core.GameInfo.Models;

namespace L4D2PlayStats.Core.GameInfo.Repositories;

public class PlayerConnectionInfoRepository(IAzureTableStorageContext tableContext) : BaseTableStorageRepository<PlayerConnectionInfoEntity>("PlayerConnectionInfo", tableContext), IPlayerConnectionInfoRepository;