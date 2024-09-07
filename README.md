# SoldierWatcher

## Project Overview

**SoldierWatcher** is a WPF-based application designed for the efficient coordination and monitoring of military training exercises. By integrating real-time sensor data, the application updates and visualizes soldier movements on a map. The primary goal is to provide evaluators with a dynamic view of soldier positions during training exercises, ensuring seamless tracking and analysis.

## Background

Efficient coordination and monitoring of military training exercises are crucial for the preparedness and effectiveness of friendly countries' armed forces. Existing systems often lack the necessary features to comprehensively register participating countries, soldiers, and ranks for training exercises. This project addresses these gaps by introducing a real-time simulation module that showcases soldier movements during training based on sensor data.

## Problem Statement

The *SoldierWatcher* system aims to provide evaluators with real-time updates on soldier movements during training exercises. The system dynamically updates soldier positions on a map based on GPS coordinates as they move from one location to another, and it stores these updates in a database. Additionally, users can click on map markers to access detailed information about soldiers, including:

- Current position (latitude and longitude)
- Rank
- Country
- Training information

The system emphasizes optimized performance, particularly when handling a substantial volume of markers or when frequent updates to marker positions are required.

## Features

- Real-time soldier position updates on a map based on sensor data.
- Marker-based interaction: Users can click on markers to view detailed information about soldiers.
- High-performance updates, designed to handle numerous markers and frequent position changes efficiently.
- Data storage: Soldier movements and details are stored in a database for future analysis.

## Technologies Used

- **WPF**: For building the user interface and handling the main application logic.
- **CommunityToolkit**: Provides MVVM support and other utilities to simplify WPF development.
- **GMap.NET**: Used for rendering maps and managing markers representing soldier positions.
- **xUnit**: For unit testing the application.
- **NSubstitute**: A mocking framework used in unit tests.
- **FluentAssertions**: Provides an elegant syntax for asserting the expected results in unit tests.

## Getting Started

### Prerequisites

To run this project, ensure you have the following installed:

- [.NET SDK](https://dotnet.microsoft.com/download)
- A compatible IDE like [Visual Studio](https://visualstudio.microsoft.com/) or [Rider](https://www.jetbrains.com/rider/)

### Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/ruisantos78/SoldierWatcher
   ```

2. Navigate to the project directory:

   ```bash
   cd SoldierWatcher
   ```

3. Restore the required NuGet packages:

   ```bash
   dotnet restore
   ```

4. Build the project:

   ```bash
   dotnet build
   ```

### Running the Application

To run the application, execute the following command:

```bash
dotnet run
```

Alternatively, you can run the application directly from your IDE.

## Testing

Unit tests have been implemented using xUnit, NSubstitute, and FluentAssertions. To execute the tests, run:

```bash
dotnet test
```

## Usage

Once the application is running:

1. You will see a map with markers representing soldiers' positions.
2. Markers will automatically update as sensor data is received.
3. Click on a marker to view detailed information about the soldier.
4. The application will save soldier movements and details to the log file.