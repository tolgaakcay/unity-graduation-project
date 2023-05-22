import numpy as np
import pandas as pd
import matplotlib.pyplot as plt

# Read the CSV file
data = pd.read_csv('heatmap_data.csv')
x_positions = data['x'].values
y_positions = data['y'].values

# Define grid parameters
grid_size = 100
grid_resolution = 0.1
agent_density = np.zeros((grid_size, grid_size))

# Convert positions to grid indices
grid_indices = ((x_positions / grid_resolution).astype(int),
                (y_positions / grid_resolution).astype(int))

# Update agent density grid
for x, y in zip(*grid_indices):
    agent_density[y, x] += 1

# Create density heatmap
plt.imshow(agent_density, cmap='hot', origin='lower')
plt.colorbar(label='Agent Density')
plt.title('Agent Density Heatmap')
plt.xlabel('X')
plt.ylabel('Y')
plt.show()
