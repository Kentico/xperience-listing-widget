using NUnit.Framework;

using DancingGoat.Widgets;

namespace DancingGoatCore.Widgets
{
    [TestFixture]
    public class ListingWidgetBasePageViewModelTests
    {
        [Test]
        public void Ctor_ValidString_SetDocumentName()
        {
            var documentName = "Document name";

            var model = new ListingWidgetBasePageViewModel(documentName);

            Assert.That(model.DocumentName, Is.EqualTo(documentName));
        }


        [Test]
        public void Ctor_Null_ThrowsArgumentNullException()
        {
            Assert.That(() => new ListingWidgetBasePageViewModel(null), Throws.ArgumentNullException);
        }
    }
}
