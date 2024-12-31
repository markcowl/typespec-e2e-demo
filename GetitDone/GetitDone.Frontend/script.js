import createClient from '../clients/typescript/dist/browser/index.js';

const client = createClient({ endpoint: 'http://localhost:5091' });
let selectedProjectId = null;

const projectList = document.getElementById('project-list');
const taskList = document.getElementById('task-list');
const newTaskContentInput = document.getElementById('new-task-content');
const addTaskButton = document.getElementById('add-task-button');
const newProjectNameInput = document.getElementById('new-project-name');
const addProjectButton = document.getElementById('add-project-button');
const taskHeader = document.getElementById('task-header');

async function fetchProjects() {
    const response = await client.path('/projects').get();
    if (response.status === '200') {
        projectList.innerHTML = '';
        response.body.forEach(project => {
            const li = document.createElement('li');
            li.textContent = project.name;
            const selectButton = document.createElement('button');
            selectButton.textContent = 'Select';
            selectButton.onclick = () => selectProject(project.id, project.name);
            const deleteButton = document.createElement('button');
            deleteButton.textContent = 'Delete';
            deleteButton.onclick = () => deleteProject(project.id);
            li.appendChild(selectButton);
            li.appendChild(deleteButton);
            projectList.appendChild(li);
        });
    } else {
        console.error('Failed to fetch projects:', response);
    }
}

async function selectProject(projectId, projectName) {
    selectedProjectId = projectId;
    taskHeader.textContent = `Tasks for ${projectName}`;
    await fetchTasks();
}

async function fetchTasks() {
    if (!selectedProjectId) {
        taskList.innerHTML = '';
        return;
    }
    const response = await client.path('/todoitems').get({ queryParameters: { project_id: selectedProjectId } });
    if (response.status === '200') {
        taskList.innerHTML = '';
        response.body.forEach(task => {
            const li = document.createElement('li');
            li.textContent = task.content;
            const completeButton = document.createElement('button');
            completeButton.textContent = task.is_completed ? 'Reopen' : 'Complete';
            completeButton.onclick = () => toggleTaskCompletion(task.id, task.is_completed);
            const deleteButton = document.createElement('button');
            deleteButton.textContent = 'Delete';
            deleteButton.onclick = () => deleteTask(task.id);
            li.appendChild(completeButton);
            li.appendChild(deleteButton);
            taskList.appendChild(li);
        });
    } else {
        console.error('Failed to fetch tasks:', response);
    }
}

async function toggleTaskCompletion(taskId, isCompleted) {
    const path = `/todoitems/${taskId}/${isCompleted ? 'reopen' : 'close'}`;
    const response = await client.path(path).post();
    if (response.status === '204') {
        await fetchTasks();
    } else {
        console.error('Failed to toggle task completion:', response);
    }
}

async function deleteTask(taskId) {
    const response = await client.path(`/todoitems/${taskId}`).delete();
    if (response.status === '204') {
        await fetchTasks();
    } else {
        console.error('Failed to delete task:', response);
    }
}

async function addProject() {
    const projectName = newProjectNameInput.value.trim();
    if (!projectName) return;
    const response = await client.path('/projects').post({ body: { name: projectName } });
    if (response.status === '201') {
        newProjectNameInput.value = '';
        await fetchProjects();
    } else {
        console.error('Failed to add project:', response);
    }
}

async function deleteProject(projectId) {
    const response = await client.path(`/projects/${projectId}`).delete();
    if (response.status === '204') {
        await fetchProjects();
        taskList.innerHTML = '';
        taskHeader.textContent = 'Tasks';
        selectedProjectId = null;
    } else {
        console.error('Failed to delete project:', response);
    }
}

async function addTask() {
    if (!selectedProjectId) {
        alert('Please select a project first.');
        return;
    }
    const taskContent = newTaskContentInput.value.trim();
    if (!taskContent) return;
    const response = await client.path('/todoitems').post({ body: { content: taskContent, project_id: selectedProjectId } });
    if (response.status === '201') {
        newTaskContentInput.value = '';
        await fetchTasks();
    } else {
        console.error('Failed to add task:', response);
    }
}

addProjectButton.addEventListener('click', addProject);
addTaskButton.addEventListener('click', addTask);

fetchProjects();