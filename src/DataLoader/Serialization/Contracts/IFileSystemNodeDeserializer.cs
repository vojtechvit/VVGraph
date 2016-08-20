﻿using Domain.Model;

namespace DataLoader.Serialization.Contracts
{
    public interface IFileSystemNodeDeserializer
    {
        Node Deserialize(string path);
    }
}