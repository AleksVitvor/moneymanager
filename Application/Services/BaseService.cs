namespace Application.Services
{
    using AutoMapper;
    using Persistence;

    public class BaseService
    {
        public readonly MoneyManagerContext context;
        public readonly IMapper mapper;
        public BaseService(MoneyManagerContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
    }
}
