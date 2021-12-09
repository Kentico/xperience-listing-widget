using NUnit.Framework;

namespace DancingGoat.Widgets
{
    [TestFixture]
    public class ListingWidgetBasePageViewModelTests
    {
        [Test]
        public void Ctor_ValidDocumentName_SetsDocumentName()
        {
            var documentName = "Document name";

            var model = new ListingWidgetPageViewModel(documentName);

            Assert.That(model.DocumentName, Is.EqualTo(documentName));
        }


        [Test]
        public void Ctor_Null_ThrowsArgumentNullException()
        {
            Assert.That(() => new ListingWidgetPageViewModel(null), Throws.ArgumentNullException);
        }
    }
}
