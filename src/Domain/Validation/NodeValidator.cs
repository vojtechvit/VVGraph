﻿using Domain.Validation.Contracts;
using System;

namespace Domain.Validation
{
    public sealed class NodeValidator : INodeValidator
    {
        public ValidationResult ValidateId(int id)
        {
            var result = new ValidationResult();

            if (id <= 0)
            {
                result.AddError("Node id must be greater than 0.");
            }

            return result;
        }

        public ValidationResult ValidateLabel(string label)
        {
            var result = new ValidationResult();

            if (label == null)
            {
                result.AddError("Node label is required.");
            }

            if (label == string.Empty)
            {
                result.AddError("Node label must not be empty.");
            }

            return result;
        }

        public ValidationResult ValidateAdjacency(int startNodeId, int endNodeId)
        {
            var result = new ValidationResult();

            if (startNodeId == endNodeId)
            {
                result.AddError("A node cannot be adjacent to itself.");
            }

            return result;
        }
    }
}