# ACC Telemetry Analysis with LLMs

## 🏎️ Project Overview

This project builds a comprehensive system to extract, analyze, and visualize telemetry data from **Assetto Corsa Competizione (ACC)**, leveraging Large Language Models (LLMs) to provide actionable driver insights and coaching feedback. Optional integration with **Unreal Engine** enables immersive 3D visualization and VR playback of laps.

---

## Features

* **Real-time telemetry extraction** via ACC Shared Memory API and MoTeC log parsing
* **Feature engineering** to transform raw data into meaningful metrics
* **LLM-powered analysis** for natural language insights and coaching advice
* **Interactive dashboards** for lap comparisons and telemetry visualization
* **Optional Unreal Engine integration** for 3D replay and immersive driver feedback

---

## 1. Telemetry Data Collection

### Data Sources

* Shared Memory API (real-time car telemetry)
* MoTeC `.ld` log files (post-session detailed data)
* UDP broadcast (basic telemetry)
* Third-party tools and custom loggers

### Example Data Points

* Vehicle state: speed, gear, RPM, throttle/brake/clutch/steering inputs
* Track position: coordinates, sector and turn IDs
* Tire data: temperatures, pressures, wear
* Lap times, sector splits, and deltas
* Driver input time series

---

## 2. Using LLMs for Insight

LLMs like GPT-4 excel at interpreting processed telemetry data to explain performance, suggest improvements, and coach the driver:

* Interpret summaries & high-level patterns (not raw data)
* Generate natural language explanations of driving behavior
* Provide coaching and performance recommendations

### Sample LLM Input Structure

```json
{
  "turn": "T4",
  "lap_time": "2:01.234",
  "brake_point_delta": "+30m",
  "apex_speed": "12 km/h lower",
  "throttle_application": "delayed by 0.4s",
  "exit_speed": "15 km/h slower"
}
```

### Example Prompts

* "Why was Turn 4 slower on Lap 7 than Lap 3?"
* "What are the top 3 areas to improve on this track?"
* "Generate a coaching summary of this stint."

---

## 3. Data Processing & Feature Engineering

* Extract and normalize telemetry into meaningful features
* Compute deltas against optimal laps or AI baselines
* Segment laps by braking zones, apexes, and exits
* Generate metrics like braking points, throttle curves, steering angles, lap-time deltas, tire stress behavior, and driver consistency

---

## 4. Visualization Layer

If not using Unreal Engine, build interactive dashboards with tools like:

* **Plotly Dash** (Python)
* **React + D3** or **Chart.js**

Typical visualizations include:

* Lap time and sector comparisons
* Braking and throttle overlays
* Delta time graphs vs optimal laps

---

## 5. Unreal Engine Integration (Optional)

* 3D lap reconstruction with telemetry-driven car paths and physics
* Visual overlays for braking zones, throttle/brake pressures
* Commentary bubbles for coaching tips ("Too early brake", "Better apex")
* Free camera and VR modes for immersive lap analysis

### Integration Flow

1. Backend processes telemetry and extracts insights
2. Sends event markers and feedback with timestamps/positions
3. Unreal loads car paths and displays overlays for visualization

---

## 6. Software Architecture

```
+------------------+
|   ACC Game/API   |
+--------+---------+
         |
         v
+------------------+
| Telemetry Logger |
| (Shared Memory   |
|  or MoTeC Parser)|
+--------+---------+
         |
         v
+------------------+     +------------------+
| Telemetry Parser | --> | Feature Extractor|
+--------+---------+     +--------+---------+
         |                        |
         v                        v
+------------------+     +------------------+
|     Database     |<--> |  LLM Prompt Gen  |
+--------+---------+     +--------+---------+
         |                        |
         v                        v
+------------------+     +------------------+
| Visualization UI |<--> | OpenAI or Local  |
| (React/Dash/UE5) |     | LLM API          |
+------------------+     +------------------+
```

---

## 7. Example Use Cases

* **Lap Comparison**: Identify time losses and their causes per sector/turn
* **Coaching Summary**: Suggest targeted improvements on braking, throttle, and cornering
* **Driver Profiling**: Track performance trends and consistency over sessions
* **VR Playback**: Relive laps in 3D with dynamic coaching overlays

---

## Next Steps

Would you like assistance with:

* Prototype LLM prompt examples?
* Unreal Engine playback demo planning?
* Designing a real-time telemetry ingestion pipeline?

---

*Feel free to contribute or request features!*

---

If you want, I can help you draft those next steps or generate sample code snippets for any part. Just ask!
