using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Application.Interfaces.UnitOfWorks;
using EduPortal.Domain.Entities;
namespace EduPortal.Application.Services
{
    public class OutboxMessageProcessor
    {
        private readonly IGenericRepository<OutboxMessage, int> _outboxMessageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OutboxMessageProcessor(IGenericRepository<OutboxMessage, int> outboxMessageRepository, IUnitOfWork unitOfWork)
        {
            _outboxMessageRepository = outboxMessageRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task ChangeOutboxState(string message)
        {
            // Outbox mesajlarını getir
            var outboxMessages = _outboxMessageRepository.Where(x => x.Payload == message).ToList();

            // Her bir mesajı işle
            foreach (var item in outboxMessages)
            {
                item.IsProcessed = true;
                // Burada güncelleme işlemlerini biriktiriyoruz
            }

            // Değişiklikleri birleştir ve veritabanını güncelle
            _outboxMessageRepository.UpdateRange(outboxMessages);

            // Değişiklikler varsa, commit et
            if (outboxMessages.Any())
            {
                await _unitOfWork.CommitAsync();
            }
        }
    }
   
}
