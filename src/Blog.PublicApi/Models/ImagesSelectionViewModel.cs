﻿
using Blog.Domain;
using Blog.Domain.Entities;

namespace MyBlogOnCore.Models;

public class ImagesSelectionViewModel
{
    public ImagesSelectionViewModel(List<Image> images)
    {
        Images = images;
    }

    public List<Image> Images { get; set; }
}
