document.addEventListener("DOMContentLoaded", function () {
    let grid = GridStack.init({ cellHeight: 100, animate: true });

    const widgetContainer = document.getElementById("widgetContainer");

    // Define available chart widgets
    const chartWidgets = [
        { id: "membershipPieChart", width: 2, height: 2, name: "Membership Pie Chart" },
        { id: "cityBarChart", width: 2, height: 2, name: "City Bar Chart" },
        { id: "MemberJoinLineChart1", width: 2, height: 2, name: "Member Join Line Chart 1" },
        { id: "MemberJoinLineChart2", width: 2, height: 2, name: "Member Join Line Chart 2" }
    ];

    // Object to map chart IDs to their initialization functions
    const chartInitializers = {
        "cityBarChart": initializeCityBarChart,
        "membershipPieChart": initializePieChart,
        "MemberJoinLineChart1": initializeMemberJoinLineChart,
        "MemberJoinLineChart2": initializeMemberJoinLineChart
    };

    // Function to dynamically create and return a canvas inside a div
    function createChartContainer(chartId) {
        let container = document.createElement("div");
        container.classList.add("grid-stack-item-content");

        let canvas = document.createElement("canvas");
        canvas.id = chartId;  // This ID must match the one passed to initializeChart
        container.appendChild(canvas);

        return container;
    }


    // Function to create widget divs dynamically
    // Function to create widget divs dynamically
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
        // Create the chart container with the chartId
        let chartContainer = createChartContainer(chart.id);
        widget.appendChild(chartContainer);

        // Initialize the chart after the widget is added to the DOM
        setTimeout(() => initializeChart(chart.id), 100); // Delay to ensure the canvas is in the DOM
        return widget;
    }


    // Append all widgets to the container
    chartWidgets.forEach(chart => {
        widgetContainer.appendChild(createWidget(chart));
    });

    // Toggle customization panel
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

    // Function to add widget to dashboard
    window.addToDashboard = function (el) {
        let chartId = el.getAttribute("data-chart-id");

        // Create new grid item for the chart
        let newItem = document.createElement("div");
        newItem.classList.add("grid-stack-item");
        newItem.setAttribute("data-gs-width", el.getAttribute("data-gs-width"));
        newItem.setAttribute("data-gs-height", el.getAttribute("data-gs-height"));

        // Dynamically create and append a new chart container
        let chartContainer = createChartContainer(chartId);
        newItem.appendChild(chartContainer);
        grid.makeWidget(newItem);

        // Append the newItem to the dashboard
        document.getElementById("dashboard").appendChild(newItem);

        // Wait for the element to be in the DOM before initializing the chart
        setTimeout(() => initializeChart(chartId), 100);

        // Hide the selected widget from the panel
        el.style.display = "none";
    };

    // Generic function to initialize any chart by ID
    function initializeChart(chartId) {
        const initFunction = chartInitializers[chartId];
        if (initFunction) {
            initFunction(chartId);
        } else {
            console.error(`No initializer found for chart ID: ${chartId}`);
        }
    }

    // Function to generate a random color
    function getRandomColor() {
        const hue = Math.floor(Math.random() * 360);
        return `hsl(${hue}, 100%, 60%)`;
    }

    // Function to initialize a bar chart
    function initializeCityBarChart(chartId) {
        if (!window.cityCounts || !Array.isArray(window.cityCounts)) {
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
                    legend: { display: true, position: "bottom" }
                }
            }
        });
    }

    // Function to initialize a pie chart
    function initializePieChart(chartId) {
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
                    legend: { position: "right" }
                }
            }
        });
    }

    // Function to initialize a line chart
    function initializeMemberJoinLineChart(chartId) {
        if (!window.MembersAddress) {
            console.error("MembersAddress data is missing or not available.");
            return;
        }

        const labels = MembersAddress.map(item => item.city);
        const data = MembersAddress.map(item => item.count);

        const ctx = document.getElementById(chartId).getContext("2d");
        new Chart(ctx, {
            type: "line",
            data: {
                labels,
                datasets: [{
                    label: "Member According to Cities",
                    data,
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
                    legend: {
                        display: true,
                        position: "bottom"
                    }
                }
            }
        });
    }
});
