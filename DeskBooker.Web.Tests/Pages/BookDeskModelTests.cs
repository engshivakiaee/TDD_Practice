using DeskBooker.Core.Domain;
using DeskBooker.Core.Processor;
using DeskBooker.Web.Pages;
using Moq;

namespace DeskBooker.Web.Tests.Pages
{
    public class BookDeskModelTests
    {
        private Mock<IDeskBookingRequestProcessor> _processorMock;
        private DeskBookingModel _deskBookingModel;
        private DeskBookingResult _deskBookingResult;

        public BookDeskModelTests()
        {
            _processorMock = new Mock<IDeskBookingRequestProcessor>();
            _deskBookingModel = new DeskBookingModel(_processorMock.Object)
            {
                DeskBookerRequest = new DeskBookerRequest(),
            };

            _deskBookingResult = new DeskBookingResult() { Code = DeskBookingResultCode.Success };
            _processorMock.Setup(x => x.BookDesk(_deskBookingModel.DeskBookerRequest))
                .Returns(_deskBookingResult);
        }

        [Test]
        public void should_call_book_desk_method_of_processor()
        {
            // Arrange
            _deskBookingResult.Code = DeskBookingResultCode.Success;

            // Act
            _deskBookingModel.OnPost();

            // Assert
            _processorMock.Verify(x => x.BookDesk(_deskBookingModel.DeskBookerRequest), Times.Once);
        }

        [TestCase(1, true)]
       // [TestCase(0, false)]
        public void should_call_book_desk_method_of_processor_if_model_is_valid(int expectedBookDeskCall, bool modelIsValid)
        {
            // Arrange
            _deskBookingResult.Code = DeskBookingResultCode.Success;

            if (!modelIsValid)
                _deskBookingModel.ModelState.AddModelError("ErrorKey", "ErrorDesc");

            // Act
            _deskBookingModel.OnPost();

            // Assert
            _processorMock.Verify(x => x.BookDesk(_deskBookingModel.DeskBookerRequest), Times.Exactly(expectedBookDeskCall));
        }

        [Test]
        public void should_add_model_error_if_desk_is_not_available()
        {
            // Arrange
            _deskBookingResult.Code = DeskBookingResultCode.NoDeskAvailable;

            // Act
            _deskBookingModel.OnPost();

            // Assert
            Assert.IsTrue(_deskBookingModel.ModelState.Count == 1,
               "No Desk Available in this date");
        }

        [TestCase(0)]
        public void should_not_add_model_error_if_desk_is_available(int modelErrorCount)
        {
            // Arrange
            _deskBookingResult.Code = DeskBookingResultCode.Success;
            // Act
            _deskBookingModel.OnPost();

            // Assert
            Assert.That(modelErrorCount, Is.EqualTo(_deskBookingModel.ModelState.Count));
        }
    }
}
