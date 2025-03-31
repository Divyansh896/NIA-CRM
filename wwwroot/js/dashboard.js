document.addEventListener("DOMContentLoaded", function () {
    let grid = GridStack.init({ cellHeight: 100, animate: true });
    const widgetContainer = document.getElementById("widgetContainer");

    const chartWidgets = [
        { id: "membershipPieChart", width: 2, height: 2, name: "Membership Pie Chart" },
        { id: "cityBarChart", width: 2, height: 2, name: "City Bar Chart" },
        { id: "MemberJoinLineChart1", width: 2, height: 2, name: "Member Join Line Chart 1" },
        { id: "SectorPieChart", width: 2, height: 2, name: "Sector Distribution Count Pie Chart" },
        { id: "SectorBarChart", width: 2, height: 2, name: "Sector Distribution Count Bar Chart" },
        { id: "TagBarChart", width: 2, height: 2, name: "Tag Distribution Count Bar Chart" }
    ];

    const chartInitializers = {
        "cityBarChart": initializeCityBarChart,
        "membershipPieChart": initializeMembershipPieChart,
        "MemberJoinLineChart1": initializeMemberJoinLineChart,
        "SectorPieChart": initializeSectorPieChart,
        "SectorBarChart": initializeSectorBarChart,
        "TagBarChart": initializeTagBarChart
    };

    function createChartContainer(chartId) {
        let container = document.createElement("div");
        container.classList.add("grid-stack-item-content");
        let canvas = document.createElement("canvas");
        canvas.id = chartId;
        container.appendChild(canvas);

        // Add context menu event listener to the canvas
        canvas.addEventListener("contextmenu", function (event) {
            event.preventDefault(); // Prevent the default context menu
            showContextMenu(event, canvas);
        });

        return container;
    }

    function createWidget(chart) {
        let widget = document.createElement("div");
        widget.classList.add("grid-stack-item");
        widget.setAttribute("data-gs-width", chart.width);
        widget.setAttribute("data-gs-height", chart.height);
        widget.setAttribute("data-chart-id", chart.id);
        widget.onclick = function () { addToDashboard(this); };

        let content = document.createElement("div");
        content.classList.add("grid-stack-item-content");
        content.innerHTML = `<strong>${chart.name}</strong>`;
        widget.appendChild(content);

        let chartContainer = createChartContainer(chart.id);
        widget.appendChild(chartContainer);

        setTimeout(() => initializeChart(chart.id), 100);
        return widget;
    }

    function showContextMenu(event, canvas) {
        event.preventDefault();

        const contextMenu = document.createElement("ul");
        contextMenu.classList.add("context-menu");

        let removeItem = document.createElement("li");
        removeItem.textContent = "Remove from Dashboard";
        removeItem.onclick = function () {
            removeFromDashboard(canvas);
            contextMenu.remove(); // Remove the context menu after action
        };

        let copyItem = document.createElement("li");
        copyItem.textContent = "Copy Image";
        copyItem.onclick = function () {
            copyChartImage(canvas);
            contextMenu.remove(); // Remove the context menu after action
        };

        let saveItem = document.createElement("li");
        saveItem.textContent = "Save Image";
        saveItem.onclick = function () {
            saveChartImage(canvas);
            contextMenu.remove(); // Remove the context menu after action
        };

        contextMenu.appendChild(removeItem);
        contextMenu.appendChild(copyItem);
        contextMenu.appendChild(saveItem);

        document.body.appendChild(contextMenu);
        contextMenu.style.left = `${event.pageX}px`;
        contextMenu.style.top = `${event.pageY}px`;

        document.addEventListener("click", function closeContextMenu() {
            contextMenu.remove();
            document.removeEventListener("click", closeContextMenu);
        });
    }

    function addToDashboard(el) {
        let chartId = el.getAttribute("data-chart-id");

        let newItem = document.createElement("div");
        newItem.classList.add("grid-stack-item");
        newItem.setAttribute("data-gs-width", el.getAttribute("data-gs-width"));
        newItem.setAttribute("data-gs-height", el.getAttribute("data-gs-height"));

        let chartContainer = createChartContainer(chartId);
        newItem.appendChild(chartContainer);
        grid.makeWidget(newItem);
        document.getElementById("dashboard").appendChild(newItem);

        setTimeout(() => initializeChart(chartId), 100);
        el.style.display = "none";
    }

    function removeFromDashboard(canvas) {
        let widget = canvas.closest(".grid-stack-item");
        widget.style.display = "block"; // Show it again in the available widgets
        widgetContainer.appendChild(widget);
        widget.parentElement.removeChild(widget);
    }

    function copyChartImage(canvas) {
        canvas.toBlob(function (blob) {
            const item = new ClipboardItem({ "image/png": blob });
            navigator.clipboard.write([item]).then(() => {
                alert("Chart image copied to clipboard!");
            }).catch(err => {
                console.error("Failed to copy chart image: ", err);
            });
        });
    }

    function saveChartImage(canvas) {
        let link = document.createElement("a");
        link.href = canvas.toDataURL("image/png");
        link.download = canvas.id + ".png";
        link.click();
    }

    chartWidgets.forEach(chart => {
        widgetContainer.appendChild(createWidget(chart));
    });

    document.getElementById("customizeBtn").addEventListener("click", function () {
        document.getElementById("widgetPanel").style.display = "block";
        document.getElementById("saveLayoutBtn").style.display = "inline-block";
        this.style.display = "none";
    });

    document.getElementById("saveLayoutBtn").addEventListener("click", function () {
        document.getElementById("widgetPanel").style.display = "none";
        document.getElementById("customizeBtn").style.display = "inline-block";
        this.style.display = "none";
    });

    function initializeChart(chartId) {
        const initFunction = chartInitializers[chartId];
        if (initFunction) {
            initFunction(chartId);
        } else {
            console.error(`No initializer found for chart ID: ${chartId}`);
        }
    }

    function getRandomColor() {
        const hue = Math.floor(Math.random() * 360);
        return `hsl(${hue}, 100%, 60%)`;
    }

    function initializeCityBarChart(chartId) {
        if (!window.CityCounts) {
            console.error("CityCounts data is missing or not in the expected format.");
            return;
        }

        const labels = cityCounts.map(item => item.city);
        const data = cityCounts.map(item => item.count);
        const backgroundColors = data.map(() => getRandomColor());

        const ctx = document.getElementById(chartId).getContext("2d");
        new Chart(ctx, {
            type: "bar",
            data: {
                labels,
                datasets: [{
                    label: "Members in Each City",
                    data,
                    backgroundColor: backgroundColors,
                    borderColor: backgroundColors,
                    borderWidth: 1
                }]
            },
            options: {
                responsive: true,
                plugins: {
                    tooltip: { enabled: true },
                    legend: { display: true, position: "bottom" },
                    title: {
                        display: true,
                        text: 'Members In Each City Distribution', // The chart title
                        font: {
                            size: 16
                        },
                        padding: {
                            top: 10,
                            bottom: 10
                        }
                    }
                }
            }
        });
    }

    function initializeMembershipPieChart(chartId) {
        if (!window.MembershipCount) {
            console.error("MembershipCount data is missing or not available.");
            return;
        }

        const labels = MembershipCount.map(item => item.membershipType.typeName);
        const data = MembershipCount.map(item => item.count);
        const backgroundColors = data.map(() => getRandomColor());

        const ctx = document.getElementById(chartId).getContext("2d");
        new Chart(ctx, {
            type: "doughnut",
            data: {
                labels,
                datasets: [{
                    label: "Memberships Count",
                    data,
                    backgroundColor: backgroundColors,
                    borderColor: backgroundColors,
                    borderWidth: 1,
                    spacing: 5
                }]
            },
            options: {
                responsive: true,
                cutoutPercentage: 70,
                hoverOffset: 20,
                plugins: {
                    tooltip: { enabled: true },
                    legend: { position: "right" },
                    title: {
                        display: true,
                        text: 'Memberships Distribution', // The chart title
                        font: {
                            size: 18
                        },
                        padding: {
                            top: 10,
                            bottom: 10
                        }
                    }
                }
            }
        });
    }

    
    function initializeMemberJoinLineChart(chartId) {
        if (!window.MembersJoins) {
            console.error("MembersJoining data is missing or not available.");
            return;
        }

        const labels = MembersJoins.map(item => item.month);
        const data = MembersJoins.map(item => item.count);

        const ctx = document.getElementById(chartId).getContext("2d");
        new Chart(ctx, {
            type: "line",
            data: {
                labels,
                datasets: [{
                    label: "Member Joining",
                    data: data,
                    borderWidth: 2,
                    tension: 0.8
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                animation: {
                    duration: 1000,
                    easing: "easeInOutQuad"
                },
                plugins: {
                    tooltip: { enabled: true },

                    legend: {
                        display: true,
                        position: "bottom"
                    },
                    title: {
                        display: true,
                        text: 'Member Joining Distribution', // The chart title
                        font: {
                            size: 16
                        },
                        padding: {
                            top: 10,
                            bottom: 10
                        }
                    }
                }
            }
        });
    }

    function initializeSectorPieChart(chartId) {
        if (!window.SectorCounts) {
            console.error("Sector data is missing or not in the expected format.");
            return;
        }

        // Extract labels (Sector Names) and data (Counts) from the passed sector data
        const labels = SectorCounts.map(item => item.sectorName);  // Sector names
        const data = SectorCounts.map(item => item.count);  // Counts per sector

        // Generate random background colors for each sector
        const backgroundColors = data.map(() => getRandomColor());  // Random colors for each sector

        // Get the context for the chart
        const ctx = document.getElementById(chartId).getContext("2d");

        // Create the pie chart
        new Chart(ctx, {
            type: "doughnut",  // Doughnut chart type (can also use 'pie' if preferred)
            data: {
                labels: labels,
                datasets: [{
                    label: "Members per Sector",
                    data: data,
                    backgroundColor: backgroundColors,
                    borderColor: backgroundColors,
                    borderWidth: 1,
                    spacing: 5
                }]
            },
            options: {
                responsive: true,
                cutoutPercentage: 70,  // Controls the 'hole' size of the doughnut
                hoverOffset: 20,  // Spacing when hovered over a slice
                plugins: {
                    tooltip: {
                        enabled: true  // Enable tooltips for better interaction
                    },
                    legend: {
                        position: "right"  // Position the legend to the right
                    },
                    title: {
                        display: true,
                        text: 'Member Per Sector Distribution', // The chart title
                        font: {
                            size: 16
                        },
                        padding: {
                            top: 10,
                            bottom: 10
                        }
                    }
                }
            }
        });
    }

    function initializeSectorBarChart(chartId) {
        if (!window.SectorCounts) {
            console.error("Sector data is missing or not in the expected format.");
            return;
        }

        // Extract labels (Sector Names) and data (Counts) from the passed sector data
        const labels = SectorCounts.map(item => item.sectorName);  // Sector names
        const data = SectorCounts.map(item => item.count);  // Counts per sector
        const backgroundColors = data.map(() => getRandomColor());

        // Get the context for the chart
        const ctx = document.getElementById(chartId).getContext("2d");

        // Create the bar chart
        new Chart(ctx, {
            type: "bar",  // Set the chart type to 'bar' for a bar chart
            data: {
                labels: labels,  // X-axis labels (sector names)
                datasets: [{
                    label: "Members per Sector",
                    data: data,  // Y-axis data (counts)
                    backgroundColor: backgroundColors,  // Random color for the bars
                    borderWidth: 1,  // Bar border width
                    hoverBackgroundColor: backgroundColors,  // Color when hovered over
                    hoverBorderColor: backgroundColors,  // Border color when hovered over
                }]
            },
            options: {
                responsive: true,
                plugins: {
                    tooltip: {
                        enabled: true  // Enable tooltips for better interaction
                    },
                    legend: {
                        position: "top"  // Position the legend at the top (or "bottom", "right")
                    },
                    title: {
                        display: true,
                        text: 'Member Per Sector Distribution',  // Chart title
                        font: {
                            size: 16
                        },
                        padding: {
                            top: 10,
                            bottom: 10
                        }
                    }
                },
                scales: {
                    x: {
                        title: {
                            display: true,
                            text: 'Sectors'  // Label for X-axis
                        },
                        grid: {
                            display: false  // Hide gridlines for the X-axis
                        }
                    },
                    y: {
                        title: {
                            display: true,
                            text: 'Member Count'  // Label for Y-axis
                        },
                        beginAtZero: true,  // Start Y-axis from zero
                        ticks: {
                            stepSize: 1  // Adjust the step size of Y-axis ticks (optional)
                        }
                    }
                }
            }
        });
    }

    function initializeTagBarChart(chartId) {
        if (!window.TagCounts) {
            console.error("Tag data is missing or not in the expected format.");
            return;
        }

        // Extract labels (Tag Names) and data (Counts) from the passed tag data
        const labels = TagCounts.map(item => item.tagName);  // Tag names
        const data = TagCounts.map(item => item.count);  // Counts per tag

        // Generate random background colors for each sector
        const backgroundColors = data.map(() => getRandomColor());  // Random colors for each sector


        // Get the context for the chart
        const ctx = document.getElementById(chartId).getContext("2d");

        // Create the bar chart
        new Chart(ctx, {
            type: "bar",  // Set the chart type to 'bar' for a bar chart
            data: {
                labels: labels,  // X-axis labels (tag names)
                datasets: [{
                    label: "Members per Tag",
                    data: data,  // Y-axis data (counts)
                    backgroundColor: backgroundColors,  // Random color for the bars
                    borderWidth: 1,  // Bar border width
                }]
            },
            options: {
                responsive: true,
                plugins: {
                    tooltip: {
                        enabled: true  // Enable tooltips for better interaction
                    },
                    legend: {
                        position: "top"  // Position the legend at the top (or "bottom", "right")
                    },
                    title: {
                        display: true,
                        text: 'Member Per Tag Distribution',  // Chart title
                        font: {
                            size: 16
                        },
                        padding: {
                            top: 10,
                            bottom: 10
                        }
                    }
                },
                scales: {
                    x: {
                        title: {
                            display: true,
                            text: 'Tags'  // Label for X-axis
                        },
                        grid: {
                            display: false  // Hide gridlines for the X-axis
                        }
                    },
                    y: {
                        title: {
                            display: true,
                            text: 'Member Count'  // Label for Y-axis
                        },
                        beginAtZero: true,  // Start Y-axis from zero
                        ticks: {
                            stepSize: 1  // Adjust the step size of Y-axis ticks (optional)
                        }
                    }
                }
            }
        });
    }


});


document.querySelectorAll('.widget-hover').forEach(function (box) {
    box.addEventListener('mouseenter', function () {
        document.querySelectorAll('.widget-hover').forEach(function (otherBox) {
            if (otherBox !== box) {
                otherBox.classList.add('shrink');
            }
        });
        box.classList.remove('shrink');
        //console.log("widget-hover")
    });

    box.addEventListener('mouseleave', function () {
        document.querySelectorAll('.widget-hover').forEach(function (otherBox) {
            otherBox.classList.remove('shrink');
        });
        //console.log("left box")
    });
});