﻿namespace MyTested.AspNetCore.Mvc.Builders.ActionResults.Object
{
    using Base;
    using Contracts.ActionResults.Object;
    using Exceptions;
    using Internal;
    using Internal.TestContexts;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Used for testing <see cref="ObjectResult"/>.
    /// </summary>
    public class ObjectTestBuilder : BaseTestBuilderWithOutputResult<ObjectResult, IAndObjectTestBuilder>, IAndObjectTestBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectTestBuilder"/> class.
        /// </summary>
        /// <param name="testContext"><see cref="ControllerTestContext"/> containing data about the currently executed assertion chain.</param>
        public ObjectTestBuilder(ControllerTestContext testContext)
            : base(testContext)
        {
        }

        /// <summary>
        /// Gets the object result test builder.
        /// </summary>
        /// <value>Test builder of <see cref="IAndObjectTestBuilder"/>.</value>
        protected override IAndObjectTestBuilder ResultTestBuilder => this;

        /// <inheritdoc />
        public IObjectTestBuilder AndAlso() => this;
        
        /// <summary>
        /// Throws new object result assertion exception for the provided property name, expected value and actual value.
        /// </summary>
        /// <param name="propertyName">Property name on which the testing failed.</param>
        /// <param name="expectedValue">Expected value of the tested property.</param>
        /// <param name="actualValue">Actual value of the tested property.</param>
        protected override void ThrowNewFailedValidationException(string propertyName, string expectedValue, string actualValue)
            => this.ThrowNewObjectResultAssertionException(propertyName, expectedValue, actualValue);

        private void ThrowNewObjectResultAssertionException(
            string propertyName, 
            string expectedValue,
            string actualValue)
            => throw new ObjectResultAssertionException(string.Format(
                ExceptionMessages.ActionResultFormat,
                this.TestContext.ExceptionMessagePrefix,
                "object",
                propertyName,
                expectedValue,
                actualValue));
    }
}