# 📬 Distributed Notification System

This repository contains the system design and Docker Compose configuration for a **scalable, distributed Notification System** built using **.NET microservices**, **RabbitMQ**, **Kafka**, **Consul**, and supporting observability and orchestration tooling.

---

## 🧩 System Overview

This system is built using a **microservices architecture** and provides:

- 🔁 Asynchronous communication using **Kafka** (Pub/Sub)
- 🧭 Service Mesh with **Consul**
- 🗃️ SQL Server as persistent storage
- 📈 Centralized logging with **Seq**
- 📊 Kafka & UI tooling for real-time data/event pipeline monitoring
- 🐳 Fully containerized using Docker Swarm and Portainer

---

## System Design

![System Architecture](http://d27d8puvvch8fm.cloudfront.net/public/NotificationSystemDesign.drawio_page-0001.jpg)
