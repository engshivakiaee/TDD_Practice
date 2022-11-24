using DeskBooker.Core.DataInterface;
using DeskBooker.Core.Domain;
using DeskBooker.Core.Processor;
using Moq;

namespace DeskBooker.Core.Tests.Processor
{
    [TestFixture]
    public class DeskBookerRequestProcessorTests 
    {
        private readonly DeskBookingRequestProcessor _requestProcessor;
        private readonly DeskBookerRequest _deskBookerRequest;
        private readonly Mock<IDeskBookingRepository> _deskBookingRepositoryMock;
        private readonly Mock<IDeskRepository> _deskRepositoryMock;
        private readonly List<Desk> _availableDesks;
        public DeskBookerRequestProcessorTests()
        {

            _deskBookerRequest = new DeskBookerRequest()
            {
                FirstName = "Shiva",
                LastName = "Kiaee",
                Email = "Shiva.kiaee@gmail.com",
                Date = DateTime.Now,
            };

            _availableDesks = new List<Desk> { new Desk() { Id = 7 } , new Desk() { Id = 8 } };

            _deskBookingRepositoryMock = new Mock<IDeskBookingRepository>();
            _deskRepositoryMock = new Mock<IDeskRepository>();

            _deskRepositoryMock.Setup(x => x.GetAvailableDesks(_deskBookerRequest.Date))
                .Returns(_availableDesks);

            _requestProcessor = new DeskBookingRequestProcessor(_deskBookingRepositoryMock.Object, _deskRepositoryMock.Object);
        }

        [Test]
        public void should_return_desk_booking_result_with_values()
        {
            //Act
            var deskbookingResult = _requestProcessor.BookDesk(_deskBookerRequest);

            //Assert
            Assert.IsNotNull(deskbookingResult);
            Assert.That(_deskBookerRequest.FirstName, Is.EqualTo(deskbookingResult.FirstName));
            Assert.That(_deskBookerRequest.LastName, Is.EqualTo(deskbookingResult.LastName));
            Assert.That(_deskBookerRequest.Email, Is.EqualTo(deskbookingResult.Email));
            Assert.That(_deskBookerRequest.Date, Is.EqualTo(deskbookingResult.Date));
        }

        [Test]
        public void should_throw_exception_if_request_is_null()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => _requestProcessor.BookDesk(null));
            Assert.That(exception.ParamName, Is.EqualTo("request"));
        }

        [Test]
        public void should_save_desk_booking()
        {
            DeskBooking? savedDeskBooking = null;

            _deskBookingRepositoryMock.Setup(x => x.Save(It.IsAny<DeskBooking>()))
                .Callback<DeskBooking>(deskbooking =>
                {
                    savedDeskBooking = deskbooking;
                });

            _requestProcessor.BookDesk(_deskBookerRequest);

            _deskBookingRepositoryMock.Verify(x => x.Save(It.IsAny<DeskBooking>()), Times.Once);

            Assert.IsNotNull(savedDeskBooking);
            Assert.That(_deskBookerRequest.FirstName, Is.EqualTo(savedDeskBooking.FirstName));
            Assert.That(_deskBookerRequest.LastName, Is.EqualTo(savedDeskBooking.LastName));
            Assert.That(_deskBookerRequest.Email, Is.EqualTo(savedDeskBooking.Email));
            Assert.That(_deskBookerRequest.Date, Is.EqualTo(savedDeskBooking.Date));
            Assert.That(_availableDesks.First().Id, Is.EqualTo(savedDeskBooking.DeskId));
        }

        [Test]
        public void should_not_save_desk_booking_if_desk_is_not_available()
        {
            _availableDesks.Clear();

            _requestProcessor.BookDesk(_deskBookerRequest);

            _deskBookingRepositoryMock.Verify(x => x.Save(It.IsAny<DeskBooking>()), Times.Never);
        }

        [TestCase(DeskBookingResultCode.Success, true)]
        [TestCase(DeskBookingResultCode.NoDeskAvailable, false)]
        public void should_return_expected_resultcode(DeskBookingResultCode expectedResultCode, bool isDeskAvailable)
        {
            if (!isDeskAvailable)
            {
                _availableDesks.Clear();
            }

            var result = _requestProcessor.BookDesk(_deskBookerRequest);
            Assert.That(expectedResultCode, Is.EqualTo(result.Code));
        }

        [TestCase(5, true)]
        [TestCase(null, false)]
        public void should_return_expected_desk_booking_id(int? expectedDeskBookingId, bool isDeskAvailable)
        {
            if (!isDeskAvailable)
            {
                _availableDesks.Clear();
            }
            else
            {
                _deskBookingRepositoryMock.Setup(x => x.Save(It.IsAny<DeskBooking>()))
                .Callback<DeskBooking>(deskbooking =>
                {
                    deskbooking.Id = expectedDeskBookingId.Value;
                });

            }

            var result = _requestProcessor.BookDesk(_deskBookerRequest);
            Assert.That(expectedDeskBookingId, Is.EqualTo(result.DeskBookingId));
        }
    }
}
