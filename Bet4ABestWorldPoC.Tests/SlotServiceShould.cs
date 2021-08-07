using Xunit;
using FluentAssertions;
using Bet4ABestWorldPoC.Services.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Bet4ABestWorldPoC.Services.Exceptions;
using Bet4ABestWorldPoC.Repositories.Interfaces;
using Moq;
using Bet4ABestWorldPoC.Repositories.Entities;
using System.Linq;

namespace Bet4ABestWorldPoC.Services.Tests
{
    public class SlotServiceShould
    {
        private readonly Mock<ISlotRepository> _mockSlotRepository;

        private readonly SlotService _slotService;

        private readonly Slot DEFAULT_SLOT = new()
        {
            Id = 12,
            Name = "Default slot",
            RTP = 96
        };

        private readonly List<Slot> DEFAULT_SLOTS = new()
        {
            new Slot()
            {
                Id = 1,
                Name = "Slot 1",
                RTP = 90
            },
            new Slot()
            {
                Id = 2,
                Name = "Slot 2",
                RTP = 93
            },
            new Slot()
            {
                Id = 3,
                Name = "Slot 3",
                RTP = 90
            },
            new Slot()
            {
                Id = 4,
                Name = "No name",
                RTP = 92
            },
            new Slot()
            {
                Id = 5,
                Name = "No name 2",
                RTP = 95
            }
        };

        public SlotServiceShould()
        {
            _mockSlotRepository = new Mock<ISlotRepository>();

            _slotService = new SlotService(_mockSlotRepository.Object);
        }

        [Fact]
        public async void Return_a_list_of_all_slots()
        {
            _mockSlotRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(DEFAULT_SLOTS);

            var result = await _slotService.GetAllAsync();

            result.Should().BeEquivalentTo(DEFAULT_SLOTS);
        }

        [Fact]
        public async void Return_a_list_of_slots_filtered_by_name()
        {
            var name = "name";
            var expectedSlots = DEFAULT_SLOTS.Where(w => w.Name.Contains(name)).ToList();

            _mockSlotRepository.Setup(x => x.GetAllWhereAsync(w => w.Name.Contains(name))).ReturnsAsync(expectedSlots);

            var result = await _slotService.GetAllSlotThatContainsNameAsync(name);

            result.Should().BeEquivalentTo(expectedSlots);
        }

        [Fact]
        public async void Return_a_slot_by_id()
        {
            _mockSlotRepository.Setup(x => x.GetByIdAsync(DEFAULT_SLOT.Id)).ReturnsAsync(DEFAULT_SLOT);

            var result = await _slotService.GetSlotByIdAsync(DEFAULT_SLOT.Id);

            result.Should().Be(DEFAULT_SLOT);
        }

        [Fact]
        public void Return_slot_not_found_exception_when_game_does_not_exits_searching_by_id()
        {
            var invalidId = 333;

            _mockSlotRepository.Setup(x => x.GetByIdAsync(invalidId)).ReturnsAsync(null as Slot);

            Func<Task> action = async () => await _slotService.GetSlotByIdAsync(invalidId);

            action.Should().Throw<SlotNotFoundException>();
        }

    }
}
