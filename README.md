HybridCache Example in .NET 9

This repository contains an example implementation of Hybrid Caching in a .NET 9 application. Hybrid Caching combines the benefits of both in-memory caching and distributed caching to provide high performance and scalability.
Introduction

Caching is essential for building fast, scalable applications. Traditional caching options in ASP.NET Core include in-memory caching and distributed caching, each with its trade-offs. HybridCache in .NET 9 brings together the best of both approaches, adding useful features like protection against cache stampede, tag-based invalidation, and better performance monitoring.
Features

    Two-level caching (L1/L2):
        L1: Fast in-memory cache
        L2: Distributed cache (Redis, SQL Server, etc.)
    Protection against cache stampede
    Tag-based cache invalidation
    Configurable serialization
    Metrics and monitoring

Prerequisites

    .NET 9 SDK
    Redis (for distributed caching)