document.addEventListener("DOMContentLoaded", function () {
    function getRandomColor() {
        const hue = Math.floor(Math.random() * 360);
        return `hsl(${hue}, 100%, 60%)`;
    }
    function initializeCityBarChart() {
        if (!window.cityCounts || !Array.isArray(window.cityCounts)) {
            console.error("CityCounts data is missing or not in the expected format.");
            return;
        }

        // Prepare data for the chart
        const labels = cityCounts.map(item => item.city);
        const data = cityCounts.map(item => item.count);

        // Function to generate a random color
        

        // Generate colors
        const backgroundColors = data.map(() => getRandomColor());

        // Get the canvas context
        const ctx = document.getElementById("cityBarChart").getContext("2d");

        // Create the bar chart
        new Chart(ctx, {
            type: "bar",
            data: {
                labels: labels,
                datasets: [{
                    label: "Members in Each City",
                    data: data,
                    backgroundColor: backgroundColors,
                    borderColor: backgroundColors,
                    borderWidth: 1
                }]
            },
            options: {
                responsive: true,
                plugins: {
                    tooltip: { enabled: true },
                    legend: {
                        display: true,
                        position: "bottom"
                    }
                }
            }
        });
        //console.log(labels);

    }

    function initializePieChart() {
        // Prepare data for the chart
        const labels = MembershipCount.map(item => item.membershipType.typeName);
        const data = MembershipCount.map(item => item.count);

        // Function to generate a random color
       

        // Generate colors
        const backgroundColors = data.map(() => getRandomColor());

        // Get the canvas context
        const ctx = document.getElementById("membershipPieChart").getContext("2d");

        // Create the doughnut chart
        new Chart(ctx, {
            type: "doughnut", // Set chart type to doughnut
            data: {
                labels: labels,
                datasets: [{
                    label: "Memberships Count",
                    data: data,
                    backgroundColor: backgroundColors, // Background color for each slice
                    borderColor: backgroundColors, // Border color same as background
                    borderWidth: 1, // Border width for separation
                    spacing: 5 // Optional: A small gap between each arc
                }]
            },
            options: {
                responsive: true, // Make the chart responsive to screen size
                cutoutPercentage: 70, // Controls the size of the hole in the middle (larger value = smaller hole)
                //rotation: Math.PI / 4, // Optionally rotate the chart
                //circumference: Math.PI * 2, // Full circle
                hoverOffset: 20, // Create a slight space when hovered (pop-out effect)
                plugins: {
                    tooltip: {
                        enabled: true, // Enable tooltips for interactivity
                    },
                    legend: {
                        position: "right", // Position the legend at the top
                        
                    }
                }
            }
        });
    }

    function initializeMemberJoinLineChart() {
        // Prepare data for the chart
        const labels = MembersAddress.map(item => item.city);
        const data = MembersAddress.map(item => item.count);

        // Function to generate a random color
        

        // Generate colors
        const backgroundColors = data.map(() => getRandomColor());
        const borderColors = data.map(() => getRandomColor());  // Different border colors

        // Get the canvas context
        const ctx = document.getElementById("MemberJoinLineChart").getContext("2d");

        // Create the line chart
        new Chart(ctx, {
            type: "line", // Set chart type to line
            data: {
                labels: labels,
                datasets: [{
                    label: "Member According to Cities",
                    data: data,
                    //backgroundColor: backgroundColors, // Transparent background color for each point
                    //borderColor: borderColors, // Border color for each line segment
                    borderWidth: 2, // Border width for the line
                    
                    tension: 0.8 // Smooth line curve
                }]
            },
            options: {
                responsive: true, // Make the chart responsive to screen size
                maintainAspectRatio: false, // Allow flexibility in maintaining aspect ratio
                animation: {
                    duration: 1000, // Smooth animation on chart load
                    easing: "easeInOutQuad" // Smooth easing function
                },
                plugins: {
                    
                    legend: {
                        display: true, // Show legend
                        position: "bottom", // Position the legend at the bottom
                        labels: {
                            fontColor: "#333", // Dark color for legend text
                            fontSize: 14, // Font size for legend
                            padding: 10 // Padding between legend items
                        }
                    }
                }
            }
        });
    }

    initializeCityBarChart();
    initializePieChart();
    initializeMemberJoinLineChart();
    console.log(MembersJoins);

});
