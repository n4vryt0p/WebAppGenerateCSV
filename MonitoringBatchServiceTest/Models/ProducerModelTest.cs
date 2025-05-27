using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.DAL.Model;
using static AF.DAL.Model.PolicyModela;

namespace MonitoringBatchServiceTest.Models
{
    public class ProducerModelTest
    {

        // Test for root ProductModel class (empty in this case)
        [Fact]
        public void ProductModel_ShouldExist()
        {
            // Arrange & Act
            var model = new ProductModel();

            // Assert
            Assert.NotNull(model);
        }

        // Tests for ProductViewModel class
        [Fact]
        public void ProductViewModel_ShouldInitializeWithNullProperties()
        {
            // Arrange & Act
            var viewModel = new ProductModel.ProductViewModel();

            // Assert
            Assert.Null(viewModel.header);
            Assert.Null(viewModel.data);
        }

        // Tests for Header class
        [Fact]
        public void Header_ShouldInitializeWithDefaultValues()
        {
            // Arrange & Act
            var header = new ProductModel.Header();

            // Assert
            Assert.Null(header.txnLogId);
            Assert.Equal(0, header.processingTime);
        }

        // Tests for Data class
        [Fact]
        public void Data_ShouldInitializeWithNullProducts()
        {
            // Arrange & Act
            var data = new ProductModel.Data();

            // Assert
            Assert.Null(data.products);
        }

        [Fact]
        public void Data_ShouldHandleProductArray()
        {
            // Arrange
            var data = new ProductModel.Data();

            // Act
            data.products = new ProductModel.Product[2];

            // Assert
            Assert.Equal(2, data.products.Length);
        }

        // Tests for Product class
        [Fact]
        public void Product_ShouldInitializeWithNullProperties()
        {
            // Arrange & Act
            var product = new ProductModel.Product();

            // Assert
            Assert.Null(product.Product_HK);
            Assert.Null(product.SystemSRC_CD);
            Assert.Null(product.ProductCode);
            Assert.Null(product.Product_NM);
            Assert.Null(product.Currency_CD);
            Assert.Null(product.SalesEff_DT);
            Assert.Null(product.SalesExp_DT);
            Assert.Null(product.productCoverages);
        }

        // Tests for Productcoverage class
        [Fact]
        public void Productcoverage_ShouldInitializeWithDefaultValues()
        {
            // Arrange & Act
            var coverage = new ProductModel.Productcoverage();

            // Assert
            Assert.Equal(0, coverage.Coverage_ID);
            Assert.Null(coverage.CoverageCode);
            Assert.Null(coverage.Coverage_NM);
            Assert.Null(coverage.Description);
            Assert.Null(coverage.CoverageType_CD);
            Assert.Null(coverage.CoverageCategory_CD);
            Assert.Null(coverage.CoverageUnitType_CD);
            Assert.Equal(0, coverage.Value);
        }

        // Integration test for complete model structure
        [Fact]
        public void CompleteModelStructure_ShouldWorkCorrectly()
        {
            // Arrange
            var viewModel = new ProductModel.ProductViewModel
            {
                header = new ProductModel.Header
                {
                    txnLogId = "LOG12345",
                    processingTime = 150
                },
                data = new ProductModel.Data
                {
                    products = new ProductModel.Product[]
                    {
                    new ProductModel.Product
                    {
                        Product_HK = "PROD001",
                        ProductCode = "P001",
                        Product_NM = "Asuransi Kendaraan",
                        productCoverages = new ProductModel.Productcoverage[]
                        {
                            new ProductModel.Productcoverage
                            {
                                Coverage_ID = 1,
                                CoverageCode = "COV01",
                                Coverage_NM = "Pertanggungan Total",
                                Value = 500000000
                            }
                        }
                    }
                    }
                }
            };

            // Act
            var firstProduct = viewModel.data.products[0];
            var firstCoverage = firstProduct.productCoverages[0];

            // Assert
            // Header assertions
            Assert.Equal("LOG12345", viewModel.header.txnLogId);
            Assert.Equal(150, viewModel.header.processingTime);

            // Data assertions
            Assert.Single(viewModel.data.products);

            // Product assertions
            Assert.Equal("PROD001", firstProduct.Product_HK);
            Assert.Equal("P001", firstProduct.ProductCode);
            Assert.Equal("Asuransi Kendaraan", firstProduct.Product_NM);
            Assert.Single(firstProduct.productCoverages);

            // Coverage assertions
            Assert.Equal(1, firstCoverage.Coverage_ID);
            Assert.Equal("COV01", firstCoverage.CoverageCode);
            Assert.Equal("Pertanggungan Total", firstCoverage.Coverage_NM);
            Assert.Equal(500000000, firstCoverage.Value);
        }

        // Test for null scenario
        [Fact]
        public void ProductViewModel_WithNullData_ShouldNotThrow()
        {
            // Arrange
            var viewModel = new ProductModel.ProductViewModel
            {
                header = null,
                data = null
            };

            // Act & Assert
            Assert.Null(viewModel.header);
            Assert.Null(viewModel.data);
        }

        // Test for empty arrays
        [Fact]
        public void Data_WithEmptyProductsArray_ShouldHandleCorrectly()
        {
            // Arrange
            var data = new ProductModel.Data
            {
                products = new ProductModel.Product[0]
            };

            // Act & Assert
            Assert.Empty(data.products);
        }

        // Test for multiple coverages
        [Fact]
        public void Product_WithMultipleCoverages_ShouldHandleCorrectly()
        {
            // Arrange
            var product = new ProductModel.Product
            {
                productCoverages = new ProductModel.Productcoverage[]
                {
                new ProductModel.Productcoverage { Coverage_ID = 1 },
                new ProductModel.Productcoverage { Coverage_ID = 2 }
                }
            };

            // Act & Assert
            Assert.Equal(2, product.productCoverages.Length);
        }
    
}
}
