namespace SpendManagament.ReadModel.UnitTests
{
    public static class CalculadoraTests
    {
        public class TestesCalculadora
        {
            [Fact]
            public void TestarSomarNumerosPositivos()
            {
                //Assert

                Calculadora calculadora = new Calculadora();
                //Act
                int resultado = calculadora.Somar(3, 5);

                //Arrange
                Assert.Equal(8, resultado);
            }
        }
    }
}
