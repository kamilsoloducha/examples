using NUnit.Framework;
using Testing;

namespace Tests;

[TestFixture]
public class SetValuesTests
{

    [Test]
    public void SetValue_WhenFieldIsReadonlyPrivate()
    {
        // arrange
        var obj = new PrivateReadonlyField();
        
        // act
        obj.SetField("_field", "test");

        //assert
        Assert.That(obj.Field, Is.EqualTo("test"));
    }

    [Test]
    public void SetProperty_WhenPropertyDoNotHaveSetter()
    {
        // arrange
        var obj = new ReadonlyProperty();
        
        // act
        obj.SetProperty(nameof(ReadonlyProperty.Property), "test");
        
        //assert
        Assert.That(obj.Property, Is.EqualTo("test"));
    }
    
    [Test]
    public void SetProperty_WhenPropertyHasSetter()
    {
        // arrange
        var obj = new PropertyClass();
        
        // act
        obj.SetProperty(nameof(PropertyClass.Property), "test");
        
        //assert
        Assert.That(obj.Property, Is.EqualTo("test"));
    }
    
    [Test]
    public void Execute_WhenPrivateMethodReturnsValue()
    {
        // arrange
        var obj = new PrivateMethods();
        
        // act
        var result = obj.Execute("TestMethod", ["test"]);
        //assert
        Assert.That(result, Is.EqualTo("test"));
    }
}

public class PrivateReadonlyField
{
    private readonly string _field = string.Empty;
    public string Field => _field;
}

public class ReadonlyProperty
{
    public string Property { get; }
}

public class PropertyClass
{
    public string Property { get; set; }
}

public class PrivateMethods
{
    private string TestMethod(string value) => value;
}