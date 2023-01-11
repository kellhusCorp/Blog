﻿namespace MyBlogOnCore.BLL;

public interface IFileProvider
{
    Task<byte[]> GetFileAsync(string fileName);

    Task AddFileAsync(string fileName, byte[] file);

    Task DeleteFileAsync(string fileName);
}