import streamlit as st
import json
import pandas as pd
import plotly.graph_objects as go
from datetime import datetime
import time

# Page config
st.set_page_config(
    page_title="ACC Telemetry Dashboard",
    layout="wide",
    initial_sidebar_state="collapsed"
)

# Main title
st.title("ACC Telemetry Dashboard")

# Create three rows of metrics
row1_cols = st.columns(4)
row2_cols = st.columns(4)
row3_cols = st.columns(4)

# First row - Primary metrics
with row1_cols[0]:
    speed = st.metric("Speed (km/h)", "0.00")
with row1_cols[1]:
    rpms = st.metric("RPMs", "0")
with row1_cols[2]:
    gear = st.metric("Gear", "N")
with row1_cols[3]:
    fuel = st.metric("Fuel", "0.00")

# Second row - Temperatures
with row2_cols[0]:
    brake_temp = st.metric("Brake Temp (°C)", "0.00")
with row2_cols[1]:
    tyre_temp = st.metric("Tyre Core Temp (°C)", "0.00")
with row2_cols[2]:
    water_temp = st.metric("Water Temp (°C)", "0.00")
with row2_cols[3]:
    air_temp = st.metric("Air Temp (°C)", "0.00")

# Third row - Vehicle dynamics
with row3_cols[0]:
    abs_active = st.metric("ABS", "Off")
with row3_cols[1]:
    tc_active = st.metric("TC", "Off")
with row3_cols[2]:
    turbo = st.metric("Turbo Boost", "0.00")
with row3_cols[3]:
    steer = st.metric("Steer Angle", "0.00")

# Create graphs layout
st.subheader("Telemetry Graphs")
row1_graphs = st.columns(2)
row2_graphs = st.columns(2)

with row1_graphs[0]:
    speed_chart = st.empty()
with row1_graphs[1]:
    rpm_chart = st.empty()
with row2_graphs[0]:
    pedals_chart = st.empty()
with row2_graphs[1]:
    temps_chart = st.empty()

def load_telemetry():
    try:
        with open("latest_telemetry.json", "r") as f:
            return json.load(f)
    except FileNotFoundError:
        return None
    except json.JSONDecodeError:
        return None

# Initialize session state
if 'speed_history' not in st.session_state:
    st.session_state.speed_history = []
    st.session_state.rpm_history = []
    st.session_state.time_history = []
    st.session_state.gas_history = []
    st.session_state.brake_history = []
    st.session_state.clutch_history = []
    st.session_state.brake_temp_history = []
    st.session_state.tyre_temp_history = []
    st.session_state.last_update = None

def update_dashboard(telemetry):
    if telemetry['Type'] != 'Physics':
        return

    # Update primary metrics
    speed.metric("Speed (km/h)", f"{telemetry['SpeedKmh']:.2f}")
    rpms.metric("RPMs", telemetry['Rpms'])
    gear.metric("Gear", "N" if telemetry['Gear'] == 0 else str(telemetry['Gear']))
    fuel.metric("Fuel", f"{telemetry['Fuel']:.2f}")
    
    # Temperature metrics
    avg_brake = sum(telemetry['BrakeTemp']) / len(telemetry['BrakeTemp'])
    avg_tyre = sum(telemetry['TyreCoreTemp']) / len(telemetry['TyreCoreTemp'])
    
    brake_temp.metric("Brake Temp (°C)", f"{avg_brake:.1f}")
    tyre_temp.metric("Tyre Core Temp (°C)", f"{avg_tyre:.1f}")
    water_temp.metric("Water Temp (°C)", f"{telemetry['WaterTemp']:.1f}")
    air_temp.metric("Air Temp (°C)", f"{telemetry['AirTemp']:.1f}")
    
    # Vehicle dynamics metrics
    abs_active.metric("ABS", "Active" if telemetry['Abs'] > 0 else "Off")
    tc_active.metric("TC", "Active" if telemetry['TC'] > 0 else "Off")
    turbo.metric("Turbo Boost", f"{telemetry['TurboBoost']:.2f}")
    steer.metric("Steer Angle", f"{telemetry['SteerAngle']:.1f}")
    
    # Update histories
    timestamp = datetime.fromisoformat(telemetry['Timestamp'].replace('Z', '+00:00'))
    st.session_state.speed_history.append(telemetry['SpeedKmh'])
    st.session_state.rpm_history.append(telemetry['Rpms'])
    st.session_state.time_history.append(timestamp)
    st.session_state.gas_history.append(telemetry['Gas'])
    st.session_state.brake_history.append(telemetry['Brake'])
    st.session_state.clutch_history.append(telemetry['Clutch'])
    st.session_state.brake_temp_history.append(avg_brake)
    st.session_state.tyre_temp_history.append(avg_tyre)

    # Keep last 100 points
    max_points = 100
    if len(st.session_state.time_history) > max_points:
        st.session_state.speed_history.pop(0)
        st.session_state.rpm_history.pop(0)
        st.session_state.time_history.pop(0)
        st.session_state.gas_history.pop(0)
        st.session_state.brake_history.pop(0)
        st.session_state.clutch_history.pop(0)
        st.session_state.brake_temp_history.pop(0)
        st.session_state.tyre_temp_history.pop(0)

    # Update graphs
    # Speed Chart
    fig_speed = go.Figure()
    fig_speed.add_trace(go.Scatter(
        x=st.session_state.time_history,
        y=st.session_state.speed_history,
        name="Speed",
        line=dict(color='#00ff00', width=2)
    ))
    fig_speed.update_layout(
        title="Speed Over Time",
        xaxis_title="Time",
        yaxis_title="Speed (km/h)",
        height=300,
        margin=dict(l=50, r=50, t=50, b=50)
    )
    speed_chart.plotly_chart(fig_speed, use_container_width=True)

    # RPM Chart
    fig_rpm = go.Figure()
    fig_rpm.add_trace(go.Scatter(
        x=st.session_state.time_history,
        y=st.session_state.rpm_history,
        name="RPM",
        line=dict(color='#ff0000', width=2)
    ))
    fig_rpm.update_layout(
        title="Engine RPM",
        xaxis_title="Time",
        yaxis_title="RPM",
        height=300,
        margin=dict(l=50, r=50, t=50, b=50)
    )
    rpm_chart.plotly_chart(fig_rpm, use_container_width=True)

    # Pedals Chart
    fig_pedals = go.Figure()
    fig_pedals.add_trace(go.Scatter(
        x=st.session_state.time_history,
        y=st.session_state.gas_history,
        name="Throttle",
        line=dict(color='#00ff00', width=2)
    ))
    fig_pedals.add_trace(go.Scatter(
        x=st.session_state.time_history,
        y=st.session_state.brake_history,
        name="Brake",
        line=dict(color='#ff0000', width=2)
    ))
    fig_pedals.add_trace(go.Scatter(
        x=st.session_state.time_history,
        y=st.session_state.clutch_history,
        name="Clutch",
        line=dict(color='#0000ff', width=2)
    ))
    fig_pedals.update_layout(
        title="Pedal Inputs",
        xaxis_title="Time",
        yaxis_title="Position",
        height=300,
        margin=dict(l=50, r=50, t=50, b=50)
    )
    pedals_chart.plotly_chart(fig_pedals, use_container_width=True)

    # Temperatures Chart
    fig_temps = go.Figure()
    fig_temps.add_trace(go.Scatter(
        x=st.session_state.time_history,
        y=st.session_state.brake_temp_history,
        name="Brake Temp",
        line=dict(color='#ff9900', width=2)
    ))
    fig_temps.add_trace(go.Scatter(
        x=st.session_state.time_history,
        y=st.session_state.tyre_temp_history,
        name="Tyre Temp",
        line=dict(color='#9900ff', width=2)
    ))
    fig_temps.update_layout(
        title="Temperatures",
        xaxis_title="Time",
        yaxis_title="Temperature (°C)",
        height=300,
        margin=dict(l=50, r=50, t=50, b=50)
    )
    temps_chart.plotly_chart(fig_temps, use_container_width=True)

if __name__ == "__main__":
    # Add status indicator
    status = st.empty()
    
    try:
        while True:
            telemetry = load_telemetry()
            if telemetry:
                if (st.session_state.last_update is None or 
                    telemetry.get('Timestamp') != st.session_state.last_update):
                    update_dashboard(telemetry)
                    st.session_state.last_update = telemetry.get('Timestamp')
                    status.success("✅ Dashboard updated with latest telemetry")
            else:
                status.warning("⏳ Waiting for telemetry data...")
            
            time.sleep(0.1)  # 100ms refresh rate
            
    except KeyboardInterrupt:
        status.error("❌ Dashboard stopped")
    except Exception as e:
        status.error(f"❌ Error: {str(e)}")