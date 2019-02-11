using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DeleteDuplicateFiles.DataSource;
using JetBrains.Annotations;

namespace DeleteDuplicateFiles.Repositories
{
    [UsedImplicitly]
    public class FileHashRepository : RepositoryBase
    {
        private readonly IMapper _mapper;
        private readonly FileDataContext _fileDataContext;

        public FileHashRepository(IMapper mapper, FileDataContext fileDataContext)
        {
            _mapper = mapper;
            _fileDataContext = fileDataContext;
        }

        public void Save()
        {

        }
    }
}
